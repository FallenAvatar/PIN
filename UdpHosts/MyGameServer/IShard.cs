﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using MyGameServer.Data;

using Shared.Udp;

namespace MyGameServer {
	public interface IShard : IInstance, IPacketSender {
		IDictionary<uint, INetworkPlayer> Clients { get; }
		PhysicsEngine Physics { get; }
		AIEngine AI { get; }
		int CurrentPlayers => Clients.Count;
		ulong CurrentTimeLong { get; }
		uint CurrentTime { get { return unchecked((uint)CurrentTimeLong); } }
		ushort CurrentShortTime { get { return unchecked((ushort)CurrentTime); } }
		IDictionary<ushort, Tuple<Entities.IEntity, Enums.GSS.Controllers>> EntityRefMap { get; }

		void Run( CancellationToken ct );
		void Stop();
		bool Tick( double deltaTime, ulong currTime, CancellationToken ct );
		void NetworkTick( double deltaTime, ulong currTime, CancellationToken ct );
		bool MigrateOut( INetworkPlayer player );
		bool MigrateIn( INetworkPlayer player );
		ushort AssignNewRefId( Entities.IEntity entity, Enums.GSS.Controllers controller);
	}
}
