﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.Matrix {
	[MatrixMessage(Enums.MatrixPacketType.SynchronizationRequest)]
    public class SynchronizationRequest {
		[Field]
		[Length(1)]
		public IList<byte> Unk;
	}
}
