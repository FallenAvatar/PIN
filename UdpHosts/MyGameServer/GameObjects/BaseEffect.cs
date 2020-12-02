using System;
using System.Collections.Generic;
using System.Text;

using MyGameServer.Entities;

namespace MyGameServer.GameObjects {
    public class BaseEffect : BaseGameObject {
        public ulong ID { get; }

        public BaseEffect(ulong id, IEntity owner = null) : base(owner) {
            ID = id;
        }
    }
}
