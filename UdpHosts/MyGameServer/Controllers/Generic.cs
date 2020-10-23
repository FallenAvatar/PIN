using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyGameServer.Packets;
using MyGameServer.Packets.Control;

namespace MyGameServer.Controllers {
	[ControllerID(Enums.GSS.Controllers.Generic)]
	public class Generic : Base {

		public async override Task Init( INetworkPlayer player, IShard shard ) {

		}

		[MessageID((byte)Enums.GSS.Generic.Commands.ScheduleUpdateRequest)]
		public void ScheduleUpdateRequest( INetworkPlayer player, IShard shard, ulong EntityID, GamePacket packet ) {
			var req = packet.Read<Packets.GSS.Generic.ScheduleUpdateRequest>();

			player.LastRequestedUpdate = shard.CurrentTime;
			player.RequestedClientTime = Math.Max(req.requestClientTime, player.RequestedClientTime);

			if( !player.FirstUpdateRequested ) {
				player.FirstUpdateRequested = true;
				player.Respawn();
			}

			//Program.Logger.Error( "Update scheduled" );
		}

		[MessageID((byte)Enums.GSS.Generic.Commands.RequestLogout)]
		public async Task RequestLogout( INetworkPlayer player, IShard shard, ulong EntityID, GamePacket packet ) {
			var resp = new CloseConnection {
				Unk1 = 0
			};
			_ = await shard.SendTo(player, ChannelType.Control, resp);
		}
	}
}
