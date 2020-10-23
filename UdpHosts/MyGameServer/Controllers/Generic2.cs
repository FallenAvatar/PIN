using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyGameServer.Packets;

namespace MyGameServer.Controllers {
	[ControllerID(Enums.GSS.Controllers.Generic2)]
	public class Generic2 : Base {

		public async override Task Init( INetworkPlayer player, IShard shard ) {

		}

		[MessageID((byte)Enums.GSS.Generic.Commands.RequestLogout)]
		public void RequestLogout( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
		}
	}
}
