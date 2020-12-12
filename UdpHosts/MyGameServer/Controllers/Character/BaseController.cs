using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyGameServer.Packets.GSS.Character.BaseController;
using MyGameServer.Packets.GSS.Generic;

using Serilog.Core;

using Shared.Udp;

namespace MyGameServer.Controllers.Character {
	[ControllerID( Enums.GSS.Controllers.Character_BaseController )]
	public class BaseController : Base {
		public async override Task Init( INetworkPlayer player, IShard shard ) {
			_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, Test.GSS.Character.BaseController.KeyFrame.Test( player, shard ), player.EntityID );
			_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, new Packets.GSS.Character.CombatController.KeyFrame( shard ) { PlayerID = player.EntityID }, player.EntityID );
			_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, new Packets.GSS.Character.LocalEffectsController.KeyFrame( shard ) { PlayerID = player.EntityID }, player.EntityID );
			_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, new Packets.GSS.Character.MissionAndMarkerController.KeyFrame( shard ) { PlayerID = player.EntityID }, player.EntityID );
			
			_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, new CharacterLoaded(), player.EntityID );
			//_ = await shard.SendGSSTo( player, ChannelType.ReliableGss, new ResourceLocationInfosResponse(), player.EntityID );
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.FetchQueueInfo )]
		public async Task FetchQueueInfo( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.PlayerReady )]
		public async Task PlayerReady( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			player.Ready();
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.MovementInput )]
		public async Task MovementInput( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			if( packet.BytesRemaining < 64 )
				return;

			var pkt = packet.Read<MovementInput>();

			if( player == null || player.CharacterEntity == null || !player.CharacterEntity.Alive )
				return;

			player.CharacterEntity.Position = pkt.Position;
			player.CharacterEntity.Rotation = pkt.Rotation;
			player.CharacterEntity.Velocity = pkt.Velocity;
			player.CharacterEntity.AimDirection = pkt.AimDirection;
			player.CharacterEntity.MovementState = (Entities.Character.CharMovement)pkt.State;

			if( player.CharacterEntity.LastJumpTime == null )
				player.CharacterEntity.LastJumpTime = pkt.LastJumpTimer;

			//Program.Logger.Warning( "Movement Unk1: {0:X4} {1:X4} {2:X4} {3:X4} {4:X4}", pkt.UnkUShort1, pkt.UnkUShort2, pkt.UnkUShort3, pkt.UnkUShort4, pkt.LastJumpTimer );

			var resp = new ConfirmedPoseUpdate {
				ShortTime = pkt.ShortTime,
				UnkByte1 = 1,
				UnkByte2 = 0,
				Position = player.CharacterEntity.Position,
				Rotation = player.CharacterEntity.Rotation,
				State = (ushort)player.CharacterEntity.MovementState,
				Velocity = player.CharacterEntity.Velocity,
				UnkUShort1 = pkt.UnkUShort3,
				UnkUShort2 = pkt.UnkUShort4,    // Somehow affects gravity
				LastJumpTimer = pkt.LastJumpTimer,
				UnkByte3 = 0,
				NextShortTime = unchecked((ushort)(pkt.ShortTime + 90))
			};

			_ = await shard.SendGSSTo( player, ChannelType.UnreliableGss, resp, player.EntityID );

			if( player.CharacterEntity.LastJumpTime.HasValue && pkt.LastJumpTimer > player.CharacterEntity.LastJumpTime.Value )
				player.Jump();
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.SetMovementSimulation )]
		public async Task SetMovementSimulation( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.BagInventorySettings )]
		public async Task BagInventorySettings( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
		}

		[MessageID( (byte)Enums.GSS.Character.Commands.PerformTextChat )]
		public async Task PerformTextChat( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			var pkt = packet.Read<PerformTextChat>();

			Program.Logger.Information( "[{2}] {0}: {1}", player.CharacterEntity.CharData.Name, pkt.Message, pkt.Channel );

			var msgs = new ChatMessageList();
			msgs.Messages.Add( new ChatMessage {
				SenderID = player.EntityID,
				SenderName = player.CharacterEntity.CharData.Name,
				Message = pkt.Message,
				Channel = pkt.Channel == 0 ? ChatChannel.Zone : pkt.Channel
			} );

			_ = await shard.SendGSSAll( ChannelType.UnreliableGss, msgs, shard.InstanceID );
		}
	}
}