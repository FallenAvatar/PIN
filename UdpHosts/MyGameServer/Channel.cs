using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using MyGameServer.Packets;
using System.Linq;
using System.Collections.Concurrent;
using MyGameServer.Packets.Control;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace MyGameServer {
	public enum ChannelType : byte {
		Control = 0,
		Matrix = 1,
		ReliableGss = 2,
		UnreliableGss = 3,
	}

	public class Channel {
		protected struct QueueItem {
			public Packet Packet;
			public GamePacketHeader Header;
		}
		private static readonly byte[] xorByte = new byte[] { 0x00, 0xFF, 0xCC, 0xAA };
		private static readonly ulong[] xorULong = new ulong[] { 0x00, 0xFFFFFFFFFFFFFFFF, 0xCCCCCCCCCCCCCCCC, 0xAAAAAAAAAAAAAAAA };

		public static Dictionary<ChannelType, Channel> GetChannels( INetworkClient client, IPacketSender s ) {
			return new Dictionary<ChannelType, Channel>() {
				{ ChannelType.Control, new Channel(ChannelType.Control, false, false, client, s) },
				{ ChannelType.Matrix, new Channel(ChannelType.Matrix, true, true, client, s) },
				{ ChannelType.ReliableGss, new Channel(ChannelType.ReliableGss, true, true, client, s) },
				{ ChannelType.UnreliableGss, new Channel(ChannelType.UnreliableGss, true, false, client, s) },
			};
		}

		public ChannelType Type { get; protected set; }
		public bool IsSequenced { get; protected set; }
		public bool IsReliable { get; protected set; }
		public ushort CurrentSequenceNumber { get; protected set; }
		public DateTime LastActivity { get; protected set; }
		public ushort LastAck { get; protected set; }

		public delegate void PacketAvailableDelegate( GamePacket packet );
		public event PacketAvailableDelegate PacketAvailable;

		protected INetworkClient client;
		protected IPacketSender sender;
		protected ConcurrentQueue<GamePacket> incomingPackets;
		protected ConcurrentQueue<Memory<byte>> outgoingPackets;

		protected Channel( ChannelType ct, bool isSequenced, bool isReliable, INetworkClient c, IPacketSender s ) {
			Type = ct;
			IsSequenced = isSequenced;
			IsReliable = isReliable;
			client = c;
			sender = s;
			CurrentSequenceNumber = 1;
			LastAck = 0;

			incomingPackets = new ConcurrentQueue<GamePacket>();
			outgoingPackets = new ConcurrentQueue<Memory<byte>>();
		}

		public void HandlePacket( GamePacket packet ) {
			incomingPackets.Enqueue( packet );
		}

		public void Process( CancellationToken ct ) {
			while( outgoingPackets.TryDequeue( out Memory<byte> p ) ) {
				var t = new Memory<byte>(new byte[4 + p.Length]);
				p.CopyTo( t.Slice( 4 ) );
				Utils.WriteStruct( Utils.SimpleFixEndianess( client.SocketID ) ).CopyTo( t );

				_ = sender.Send( t, client.RemoteEndpoint );
				LastActivity = DateTime.Now;
			}

			while( incomingPackets.TryDequeue( out var packet ) ) {
				//Console.Write("> " + string.Concat(BitConverter.GetBytes(packet.Header.PacketHeader).ToArray().Select(b => b.ToString("X2")).ToArray()));
				//Console.WriteLine(" "+string.Concat(packet.PacketData.ToArray().Select(b => b.ToString("X2")).ToArray()));
				ushort seqNum = 0;
				if( IsSequenced ) {
					seqNum = Utils.SimpleFixEndianess( packet.Read<ushort>() );
					// TODO: Implement SequencedPacketQueue
				}

				if( packet.Header.ResendCount > 0 ) {
					// de-xor data
					int x = packet.PacketData.Length >> 3;
					var data = packet.PacketData.ToArray();

					if( x > 0 ) {
						Span<ulong> uSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, ulong>(data);

						for( int i = 0; i < x; i++ )
							uSpan[i] ^= xorULong[packet.Header.ResendCount];

						data = System.Runtime.InteropServices.MemoryMarshal.Cast<ulong, byte>( uSpan ).ToArray();
					}

					for( int i = x * 8; i < data.Length; i++ )
						data[i] ^= xorByte[packet.Header.ResendCount];

					//for( int i = 0; i < data.Length; i++ )
					//	data[i] ^= xorByte[packet.Header.ResendCount];

					packet = new GamePacket( packet.Header, new ReadOnlyMemory<byte>( data ) );
					Program.Logger.Fatal( "---> Received Resent packet!!! C:{0}: {1} bytes", Type, packet.TotalBytes );
				}

				if( packet.Header.IsSplit )
					Program.Logger.Fatal( "---> Received Split packet!!! C:{0}: {1} bytes", Type, packet.TotalBytes );

				if( IsReliable && (seqNum > LastAck || (seqNum < 0xff && LastAck > 0xff00)) ) {
					client.SendAck( Type, seqNum, packet.Recieved );
					LastAck = seqNum;
				}

				PacketAvailable?.Invoke( packet );
				LastActivity = DateTime.Now;
			}

			if( (DateTime.Now - LastActivity).TotalMilliseconds > 100 ) {
				// Send heartbeat?
			}
		}

		public async Task<bool> SendClass<T>( T pkt, Type msgEnumType = null ) where T : class {
			return await Send( GetBytes(pkt) );
		}

		public async Task<bool> SendGSSClass<T>( T pkt, ulong entityID, Enums.GSS.Controllers? controllerID = null, Type msgEnumType = null ) where T : class {
			return await Send( GetGSSBytes(pkt,entityID,controllerID) );
		}

		private const int ProtocolHeaderSize = 80; // UDP + IP
		private const int GameSocketHeaderSize = 4;
		private const int TotalHeaderSize = ProtocolHeaderSize + GameSocketHeaderSize;
		private const int MaxPacketSize = PacketServer.MTU - TotalHeaderSize;
		public async Task<bool> Send( Memory<byte> p ) {
			var hdrLen = 2;
			if( IsSequenced )
				hdrLen += 2;

			// Send UGSS messages that are split over RGSS
			if( Type == ChannelType.UnreliableGss && p.Length + hdrLen >= MaxPacketSize )
				return await client.NetChans[ChannelType.ReliableGss].Send( p );

			while( p.Length > 0 ) {
				var len = Math.Min(p.Length + hdrLen, MaxPacketSize);

				var t = new Memory<byte>(new byte[len]);
				p.Slice( 0, len - hdrLen ).CopyTo( t[hdrLen..] );

				if( IsSequenced ) {
					//if( IsReliable )
					//	Program.Logger.Verbose( "<- {0} SeqNum =  {1}", Type, CurrentSequenceNumber );

					Utils.WritePrimitive( Utils.SimpleFixEndianess( CurrentSequenceNumber ) ).CopyTo( t.Slice( 2, 2 ) );
					unchecked { CurrentSequenceNumber++; }
				}

				var hdr = new GamePacketHeader(Type, 0, (p.Length + hdrLen) > MaxPacketSize, (ushort)t.Length);
				var hdrBytes = Utils.WritePrimitive(Utils.SimpleFixEndianess(hdr.PacketHeader));
				hdrBytes.CopyTo( t );

				//Console.Write("< "+string.Concat(BitConverter.GetBytes(Utils.SimpleFixEndianess(hdr.PacketHeader)).ToArray().Select(b => b.ToString("X2")).ToArray()));
				//Console.WriteLine( " "+string.Concat( t.Slice(2).ToArray().Select( b => b.ToString( "X2" ) ).ToArray() ) )

				outgoingPackets.Enqueue( t );

				p = p[(len - hdrLen)..];
			}

			return true;
		}

		public static Memory<byte> GetBytes<T>( T pkt ) where T : class {
			Memory<byte> p;
			if( pkt is IWritable write )
				p = write.Write();
			else
				p = Utils.WriteClass( pkt );

			var controlMsgAttr = typeof(T).GetCustomAttribute<ControlMessageAttribute>();
			var matrixMsgAttr = typeof(T).GetCustomAttribute<MatrixMessageAttribute>();
			byte msgID;

			if( controlMsgAttr != null )
				msgID = (byte)controlMsgAttr.MsgID;
			else if( matrixMsgAttr != null )
				msgID = (byte)matrixMsgAttr.MsgID;
			else
				throw new Exception();

			var ret = new Memory<byte>(new byte[1 + p.Length]);
			p.CopyTo( ret[1..] );

			Utils.WritePrimitive( msgID ).CopyTo( ret );

			p = ret;

			return p;
		}

		public static Memory<byte> GetGSSBytes<T>( T pkt, ulong entityID, Enums.GSS.Controllers? controllerID = null ) where T : class {
			Memory<byte> p;
			if( pkt is IWritable write ) {
				p = write.Write();
			} else
				p = Utils.WriteClass( pkt );

			var gssMsgAttr = typeof(T).GetCustomAttribute<GSSMessageAttribute>();

			if( gssMsgAttr == null )
				throw new Exception();

			var msgID = gssMsgAttr.MsgID;
			var ret = new Memory<byte>(new byte[9 + p.Length]);
			p.CopyTo( ret[9..] );

			Utils.WritePrimitive( entityID ).CopyTo( ret );

			// Intentionally overwrite first byte of Entity ID
			if( controllerID.HasValue )
				Utils.WritePrimitive( (byte)controllerID.Value ).CopyTo( ret );
			else if( gssMsgAttr.ControllerID.HasValue )
				Utils.WritePrimitive( (byte)gssMsgAttr.ControllerID.Value ).CopyTo( ret );
			else
				throw new Exception();

			Utils.WritePrimitive( msgID ).CopyTo( ret[8..] );

			p = ret;

			return p;
		}
	}
}