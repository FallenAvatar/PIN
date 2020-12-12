using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.BaseController.PartialUpdates {
    [PartialUpdate.PartialShadowField( 0x13 )]
    public class CurrentHealth {
        [Field]
        public uint Value;
    }
}
