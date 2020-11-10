using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MyGameServer.Packets;

namespace MyGameServer.Controllers.Character {
	[ControllerID(Enums.GSS.Controllers.Character_MissionAndMarkerController)]
	public class MissionAndMarkerController : Base {
		public async override Task Init( INetworkPlayer player, IShard shard ) {

		}

		[MessageID((byte)Enums.GSS.Character.Commands.RequestAllAchievements)]
		public async Task RequestAllAchievements( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			// TODO: Implement
		}

		[MessageID((byte)Enums.GSS.Character.Commands.TryResumeTutorialChain)]
		public async Task TryResumeTutorialChain( INetworkPlayer player, IShard shard, ulong EntityID, Packets.GamePacket packet ) {
			// TODO: Implement
		}
	}
}
