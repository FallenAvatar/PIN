using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Character.BaseController {
    [GSSMessage( Enums.GSS.Controllers.Character_BaseController, (byte)Enums.GSS.Character.Events.ResourceLocationInfosResponse )]
    public class ResourceLocationInfosResponse {
        [Field]
		[Length(2)]
		public IList<byte> UnkBytes;

		public ResourceLocationInfosResponse( ) {
			UnkBytes = new byte[] { 0x00, 0x01 };
		}
	}
}
