using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyGameServer.Systems {
	public class AIEngine : ISystem {
		public SystemType SystemType { get { return SystemType.AI; } }

		public AIEngine() {

		}

		public void Tick( double deltaTime, ulong currTime, CancellationToken ct ) {

		}
	}
}
