using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;

namespace MyGameServer.Packets.GSS.Character.BaseController {
    [GSSMessage( Enums.GSS.Controllers.Character_BaseController, (byte)Enums.GSS.Character.Commands.PerformTextChat )]
    public class PerformTextChat {
        [Field]
        public string Message;
        [Field]
        public Generic.ChatChannel Channel;
    }
}
