using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Udp.Attributes {
    public sealed class LengthAttribute : Attribute {
        public int Length { get; private set; }

        public LengthAttribute( int len ) {
            Length = len;
        }
    }
}
