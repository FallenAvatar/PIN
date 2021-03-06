﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Shared.Udp;

namespace MyMatrixServer.Packets {
	[StructLayout( LayoutKind.Sequential, Pack = 1 )]
	unsafe struct MatrixPacketHehe {
		public readonly UInt32 SocketID;
		private fixed byte type[4];
		public string Type {
			get { fixed( byte* t = type ) return Utils.ReadFixedString(t, 4); }
			set { fixed( byte* t = type ) Utils.WriteFixed(t, Encoding.ASCII.GetBytes(value.Substring(0, 4))); }
		}
		public readonly UInt32 ClientSocketID;

		public MatrixPacketHehe( UInt32 clientID ) {
			SocketID = 0;
			ClientSocketID = Utils.SimpleFixEndianess(clientID);
			Type = "HEHE";
		}
	}
}
