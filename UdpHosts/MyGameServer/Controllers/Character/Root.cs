using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Controllers.Character {
	[ControllerID(Enums.GSS.Controllers.Character)]
	public class Root : Base {
		public async override Task Init( INetworkPlayer player, IShard shard ) {

		}
	}
}
