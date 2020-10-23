using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Common {
    public static class RandomExtensions {
        public static Int64 NextInt64( this Random rnd ) {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes( buffer );
            return BitConverter.ToInt64( buffer, 0 );
        }

        public static UInt64 NextUInt64( this Random rnd ) {
            var buffer = new byte[sizeof(Int64)];
            rnd.NextBytes( buffer );
            return BitConverter.ToUInt64( buffer, 0 );
        }
    }
}
