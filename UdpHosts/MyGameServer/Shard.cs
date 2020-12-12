using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MyGameServer.Data;
using MyGameServer.Entities;

using Shared.Common.Extensions;
using Shared.Udp;

namespace MyGameServer {
    public class Shard : IShard {
		
		public static IShard CurrentShard { get { return currShard; } }
		[ThreadStatic]
		private static IShard currShard;

		public const double NetworkTickRate = 1.0 / 20.0;
		protected long startTime;
		public DateTime StartTime { get { return DateTimeExtensions.Epoch.AddSeconds( startTime ); } }

		public IDictionary<uint, INetworkPlayer> Clients { get; protected set; }
		public IPacketSender NetServer { get; }
		public IDictionary<ulong, IEntity> Entities { get; protected set; }
		public IDictionary<Systems.SystemType, Systems.ISystem> Systems { get; }
		public ulong InstanceID { get; }
		public ulong CurrentTimeLong { get; protected set; }
		public IDictionary<ushort, Tuple<IEntity, Enums.GSS.Controllers>> EntityRefMap { get; }
		private ushort LastEntityRefId;
		protected Thread runThread;

		public Shard( double gameTickRate, ulong instID, IPacketSender netServer ) {
			Clients = new ConcurrentDictionary<uint, INetworkPlayer>();
			Entities = new ConcurrentDictionary<ulong, IEntity>();
			Systems = new Dictionary<Systems.SystemType, Systems.ISystem>();
			Systems.Add( MyGameServer.Systems.SystemType.AI, new Systems.AIEngine() );
			Systems.Add( MyGameServer.Systems.SystemType.Physics, new Systems.PhysicsEngine( gameTickRate ) );
			Systems.Add( MyGameServer.Systems.SystemType.Aptitude, new Systems.Aptitude() );

			NetServer = netServer;
			InstanceID = instID;
			EntityRefMap = new ConcurrentDictionary<ushort, Tuple<IEntity, Enums.GSS.Controllers>>();
			LastEntityRefId = 0;
		}

		public void Run( CancellationToken ct ) {
			runThread = Utils.RunThread( RunThread, ct );
		}

		public void Stop( ) {
			runThread.Abort();
		}

		public void RunThread( CancellationToken ct ) {
			startTime = (long)DateTime.Now.UnixTimestamp();
			var lastNetTick = 0.0;

			var sw = new Stopwatch();
			var lastTime = 0.0;
			ulong currTime;
			double delta;

			currShard = this;

			sw.Start();

			//GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

			while( !ct.IsCancellationRequested ) {
				
				//var noGC = GC.TryStartNoGCRegion( 256 * 1024 * 1024 );

				var currt = (ulong)(DateTime.Now.UnixTimestamp() * 1000);
				currTime = unchecked((ulong)sw.Elapsed.TotalMilliseconds);
				delta = currTime - lastTime;

				if( ShouldNetworkTick( currTime - lastNetTick, currt ) ) {
					NetworkTick( currTime - lastNetTick, currt, ct );
					lastNetTick = currTime;
				}

				if( !Tick( delta, currt, ct ) )
					break;

				lastTime = currTime;

				//if( noGC )
				//	GC.EndNoGCRegion();
				//GC.Collect( 1, GCCollectionMode.Forced, true, false );
				//GC.WaitForPendingFinalizers();

				_ = Thread.Yield();
			}

			sw.Stop();
		}

		public bool Tick( double deltaTime, ulong currTime, CancellationToken ct ) {
			CurrentTimeLong = currTime;
			foreach( var c in Clients.Values ) {
				if( ct.IsCancellationRequested )
					break;

				c.Tick( deltaTime, currTime, ct );
			}

			foreach( var sys in Systems ) {
				if( ct.IsCancellationRequested )
					break;

				sys.Value.Tick( deltaTime, currTime, ct );
			}

			return true;
		}

		protected virtual bool ShouldNetworkTick( double deltaTime, ulong currTime ) => deltaTime >= NetworkTickRate;
		public void NetworkTick( double deltaTime, ulong currTime, CancellationToken ct ) {
			// Handle timeout, reliable retransmission, normal rx/tx
			foreach( var c in Clients.Values ) {
				if( ct.IsCancellationRequested )
					break;

				c.NetworkTick( deltaTime, currTime, ct );
			}
		}

		public bool MigrateOut( INetworkPlayer player ) { return false; }
		public bool MigrateIn( INetworkPlayer player ) {
			if( Clients.ContainsKey( player.SocketID ) )
				return true;

			player.Init( this, NetServer );

			Clients.Add( player.SocketID, player );
			//Entities.Add(player.CharacterEntity.EntityID, player.CharacterEntity);

			return true;
		}

		public ushort AssignNewRefId( IEntity entity, Enums.GSS.Controllers controller ) {
			while( LastEntityRefId == 0 || LastEntityRefId == 0xffff || EntityRefMap.ContainsKey( LastEntityRefId ) )
				LastEntityRefId = unchecked((ushort)(LastEntityRefId + 1));

			EntityRefMap.Add( LastEntityRefId, new Tuple<IEntity, Enums.GSS.Controllers>( entity, controller ) );

			return unchecked(LastEntityRefId++);
		}

		public async Task<bool> SendAll<T>( ChannelType chan, T pkt, INetworkPlayer ignore = null ) where T : class {
			var tasks = new List<Task<bool>>();
			var p = Channel.GetBytes(pkt);

			foreach( var c in Clients.Values ) {
				if( c == ignore )
					continue;

				tasks.Add( c.NetChans[chan].Send( p ) );
			}

			return (await Task.WhenAll( tasks )).All((r) => r);
		}

		public async Task<bool> SendGSSAll<T>( ChannelType chan, T pkt, ulong entityID, Enums.GSS.Controllers? controllerID = null, INetworkPlayer ignore = null ) where T : class {
			var tasks = new List<Task<bool>>();
			var p = Channel.GetGSSBytes(pkt,entityID, controllerID);

			foreach( var c in Clients.Values ) {
				if( c == ignore )
					continue;

				tasks.Add( c.NetChans[chan].Send( p ) );
			}

			return (await Task.WhenAll( tasks )).All( ( r ) => r );
		}

		public async Task<bool> SendTo<T>( INetworkPlayer player, ChannelType chan, T pkt ) where T : class {
			return await player.NetChans[chan].SendClass( pkt );
		}

		public async Task<bool> SendGSSTo<T>( INetworkPlayer player, ChannelType chan, T pkt, ulong entityID, Enums.GSS.Controllers? controllerID = null ) where T : class {
			return await player.NetChans[chan].SendGSSClass( pkt, entityID, controllerID );
		}
	}
}
