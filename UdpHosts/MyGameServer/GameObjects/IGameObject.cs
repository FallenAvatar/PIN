using System;
using System.Collections.Generic;
using System.Text;

using MyGameServer.Entities;

namespace MyGameServer.GameObjects {
    public interface IGameObject {
        IEntity Owner { get; }
    }
}
