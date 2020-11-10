using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyGameServer.Systems {
	public class Aptitude : ISystem {
		public SystemType SystemType { get { return SystemType.Aptitude; } }

		public void Tick( double deltaTime, ulong currTime, CancellationToken ct ) {
			
		}
	}
}
