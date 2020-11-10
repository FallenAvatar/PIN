﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp;
using MyGameServer.Packets;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;
using System.Buffers;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace MyGameServer {
	class GameServer : PacketServer {
		public const double GameTickRate = 1.0 / 60.0;
		public const int MinPlayersPerShard = 16;
		public const int MaxPlayersPerShard = 64;
		public delegate void SendPacketDelegate<T>( T pkt, IPEndPoint ep ) where T : struct;

		protected ConcurrentDictionary<uint, INetworkPlayer> ClientMap;
		protected ConcurrentDictionary<ulong, IShard> Shards;

		protected byte nextShardID;
		protected ulong ServerID;

		public GameServer( ushort port, ulong serverID ) : base( port ) {
			ClientMap = new ConcurrentDictionary<uint, INetworkPlayer>();
			Shards = new ConcurrentDictionary<ulong, IShard>();

			nextShardID = 1;
			ServerID = serverID;
		}

		protected override void Startup( CancellationToken ct ) {
			Test.DataUtils.Init();
			Controllers.Factory.Init();

			_ = NewShard( ct );
		}

		protected override async void ServerRunThread( CancellationToken ct ) {
			Packet? p;
			while( (p = await incomingPackets.ReceiveAsync( ct )) != null ) {
				HandlePacket( p.Value, ct );
			}
		}

		protected IShard GetNextShard(CancellationToken ct ) {
			foreach( var s in Shards.Values.OrderBy((s) => s.CurrentPlayers) ) {
				if( s.CurrentPlayers < MaxPlayersPerShard )
					return s;
			}

			return NewShard(ct);
		}

		protected IShard NewShard(CancellationToken ct ) {
			var id = ServerID | (ulong)((nextShardID++) << 8);
			var s = Shards.AddOrUpdate( id, new Shard( GameTickRate, id, this ), ( id, old ) => old );

			s.Run(ct);

			return s;
		}

		//protected bool Tick( double deltaTime, ulong currTime, CancellationToken ct ) {
		//	foreach( var s in Shards.Values ) {
		//		if( !s.Tick( deltaTime, currTime ) || s.CurrentPlayers < MinPlayersPerShard ) {
		//			// TODO: Shutdown Shard
		//		}
		//	}

		//	return true;
		//}

		protected override void HandlePacket( Packet packet, CancellationToken ct ) {
			//Program.Logger.Information("[GAME] {0} sent {1} bytes.", packet.RemoteEndpoint, packet.PacketData.Length);
			//Program.Logger.Verbose(">  {0}", BitConverter.ToString(packet.PacketData.ToArray()).Replace("-", " "));

			var socketID = Utils.SimpleFixEndianess(packet.Read<uint>());
			INetworkClient client;

			if( !ClientMap.ContainsKey( socketID ) ) {
				var c = new NetworkPlayer(packet.RemoteEndpoint, socketID);

				client = ClientMap.AddOrUpdate( socketID, c, ( id, nc ) => { return nc; } );
				_ = GetNextShard(ct).MigrateIn( (INetworkPlayer)client );
			} else
				client = ClientMap[socketID];

			client.HandlePacket( packet.PacketData.Slice( 4 ), packet );
		}
	}
}