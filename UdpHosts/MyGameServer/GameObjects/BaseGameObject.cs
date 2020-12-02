using System;
using System.Collections.Generic;
using System.Text;

using MyGameServer.Entities;

namespace MyGameServer.GameObjects {
    public class BaseGameObject : IGameObject {
        public IEntity Owner { get; protected set; }

        public BaseGameObject( IEntity owner = null ) {
            Owner = owner;
        }
    }
}
