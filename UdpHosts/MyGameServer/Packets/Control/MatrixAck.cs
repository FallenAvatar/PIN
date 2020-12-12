using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.Control {
	[ControlMessage(Enums.ControlPacketType.MatrixAck)]
	public class MatrixAck {
		[Field]
		public ushort NextSeqNum;
		[Field]
		public ushort AckFor;
	}
}