using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.Control {
	[ControlMessage(Enums.ControlPacketType.TimeSyncResponse)]
	public class TimeSyncResponse {
		[Field]
		public ulong ClientTime;
		[Field]
		public ulong ServerTime;

		public TimeSyncResponse( ulong clientTime, ulong serverTime ) {
			ClientTime = clientTime;
			ServerTime = serverTime;
		}
	}
}
