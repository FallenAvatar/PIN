﻿using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Generic {
	[GSSMessage(Enums.GSS.Controllers.Generic, (byte)Enums.GSS.Generic.Commands.ScheduleUpdateRequest)]
	public class ScheduleUpdateRequest {
		[Field]
		public uint requestClientTime;
	}
}
