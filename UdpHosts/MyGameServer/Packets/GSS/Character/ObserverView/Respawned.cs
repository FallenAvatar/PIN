using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.ObserverView {
    [GSSMessage( Enums.GSS.Controllers.Character_ObserverView, (byte)Enums.GSS.Character.Events.Respawned )]
    public class Respawned {
        [Field]
        public uint LastUpdateTime;
        [Field]
        public ushort Unk1;
    }
}
