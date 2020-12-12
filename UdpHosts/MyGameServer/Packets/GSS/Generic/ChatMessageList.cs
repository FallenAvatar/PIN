using System;
using System.Collections.Generic;
using System.Text;

using Shared.Udp;
using Shared.Udp.Attributes;

namespace MyGameServer.Packets.GSS.Generic {
    public enum ChatChannel : byte {
        Unknown = 0,
        Zone = 1,
        Community = 2,
        Team = 3,
        Unk1 = 4,
        Encounter = 5,
        Local = 6,
        Yell = 7,
        Whisper = 8,
        Army = 9,
        Officer = 10,
        Debug = 11,
        Squad = 12,
        Platoon = 13,
        Friends = 14,
        SINSpeaker = 15,
        SINMessenger = 16,
        Admin = 17,
    }

    [GSSMessage( Enums.GSS.Controllers.Generic, (byte)Enums.GSS.Generic.Events.ChatMessageList )]
    public class ChatMessageList {
        [Field]
        [LengthPrefixed(typeof(byte))]
        public IList<ChatMessage> Messages = new List<ChatMessage>();
        // This might need to be in ChatMessage instead of here
        [Field]
        [Length(5)]
        public IList<byte> Unk = new List<byte>();
    }

    public class ChatMessage {
        [Field]
        public ulong SenderID;
        [Field]
        public string SenderName;
        [Field]
        public string Message;
        [Field]
        public ChatChannel Channel;
    }
}
