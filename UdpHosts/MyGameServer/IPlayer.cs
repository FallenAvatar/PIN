using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MyGameServer.Data;

using Shared.Udp;

namespace MyGameServer {
	public interface IPlayer {
		public enum PlayerStatus {
			Invalid = -1,
			Unknown = 0,
			Connecting = 1,
			Connected,
			LoggingIn,
			LoggedIn,
			Loading,

			Playing = 999
		}

		ulong CharacterID { get; }
		ulong EntityID { get { return CharacterEntity.EntityID; } } // Ignore last byte
		Entities.Character CharacterEntity { get; }
		PlayerStatus Status { get; }
		Zone CurrentZone { get; }
		uint LastRequestedUpdate { get; set; }
		uint RequestedClientTime { get; set; }
		bool FirstUpdateRequested { get; set; }

		void Init(IShard shard, IPacketSender s );

		Task Login( ulong charID );
		void Ready();
		void Respawn( );
		void Jump( );

		void Tick( double deltaTime, ulong currTime, CancellationToken ct );
	}
}
