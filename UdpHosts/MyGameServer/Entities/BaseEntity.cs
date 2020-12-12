using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyGameServer.Entities {
	public class BaseEntity : IEntity {
		public ulong EntityID { get; }
		public IShard Owner { get; }
		public IDictionary<Enums.GSS.Controllers, ushort> ControllerRefMap { get; }

		public BaseEntity( IShard owner ) {
			Owner = owner;
			EntityID = owner.NextEntityID;
			ControllerRefMap = new ConcurrentDictionary<Enums.GSS.Controllers, ushort>();
		}

		public void RegisterController(Enums.GSS.Controllers controller) {
			ControllerRefMap.Add(controller, Owner.AssignNewRefId(this, controller));
		}

		public virtual void Tick( double deltaTime, ulong currTime, CancellationToken ct ) {}
	}
}
