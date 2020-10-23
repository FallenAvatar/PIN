using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Controllers.Character {
	[ControllerID(Enums.GSS.Controllers.Character_CombatController)]
	public class CombatController : Base {
		public async override Task Init( INetworkPlayer player, IShard shard ) {
			// TODO: Implement

		}

		[MessageID((byte)Enums.GSS.Character.Commands.FireInputIgnored)]
		public void FireInputIgnored( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			// TODO: Implement
		}

		[MessageID((byte)Enums.GSS.Character.Commands.UseScope)]
		public void UseScope( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			// TODO: Implement
		}
	}
}
