using System;
using System.Collections.Generic;
using System.Text;

using MyGameServer.Entities;

namespace MyGameServer.Test.Entities {
    class Xsear {
        public static IEntity Get() {
            var ret = new Character(IShard.CurrentShard);
            ret.Load( 0x0022446688aacceeU );

            ret.Position = new System.Numerics.Vector3( 169f, 254f, 492f );
            ret.Alive = true;

            return ret;
        }
    }
}
