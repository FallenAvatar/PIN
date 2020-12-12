﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Udp.Attributes {
    public sealed class PaddingAttribute : Attribute {
        public int Size { get; private set; }
        public PaddingAttribute( int size ) {
            Size = size;
        }
    }
}
