using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.MovementView {
    [GSSMessage( Enums.GSS.Controllers.Character_MovementView, (byte)Enums.GSS.Character.Events.ViewKeyFrame )]
    public class KeyFrame {
		[Field]
		public float PosX;
		[Field]
		public float PosY;
		[Field]
		public float PosZ;
		[Field]
		public float RotW;
		[Field]
		public float RotX;
		[Field]
		public float RotY;
		[Field]
		public float RotZ;
		[Field]
		public float AimX;
		[Field]
		public float AimY;
		[Field]
		public float AimZ;
		[Field]
		public ushort MovementState;
		[Field]
		public uint KeyFrameTime;
	}
}
