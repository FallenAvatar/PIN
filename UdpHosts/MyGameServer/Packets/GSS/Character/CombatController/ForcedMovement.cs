﻿using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.CombatController {
    [GSSMessage( Enums.GSS.Controllers.Character_CombatController, (byte)Enums.GSS.Character.Events.ForcedMovement )]
    public class ForcedMovement {
        [Field]
        public ushort Type;
        [Field]
        public uint Unk1;
        [Field]
        public Common.Vector Position;
        [Field]
        public Common.Vector AimDirection;
        [Field]
        public Common.Vector Velocity;
        [Field]
        public uint GameTime;
        [Field]
        public ushort KeyFrame;
    }
}
