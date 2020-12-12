using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyGameServer.Entities {
	public interface IEntity {
		ulong EntityID { get; }
		IShard Owner { get; }
		IDictionary<Enums.GSS.Controllers, ushort> ControllerRefMap { get; }

		void RegisterController( Enums.GSS.Controllers controller );
		void Tick( double deltaTime, ulong currTime, CancellationToken ct );
	}
}
