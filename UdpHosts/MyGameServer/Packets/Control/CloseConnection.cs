using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp.Attributes;

namespace MyGameServer.Packets.Control {
	[ControlMessage(Enums.ControlPacketType.CloseConnection)]
	public class CloseConnection {
		[Field]
		public uint Unk1;
	}
}