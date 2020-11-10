using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyGameServer.Systems {
    public enum SystemType {
        Unknown = 0,
        AI,
        Physics,
        Aptitude
    }
    public interface ISystem {
        SystemType SystemType { get; }
        void Tick( double deltaTime, ulong currTime, CancellationToken ct );
    }
}
