using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Common.Extensions {
    public static class RandomExtensions {
        public static long NextInt64( this Random rnd ) {
            var buffer = new byte[sizeof(long)];
            rnd.NextBytes( buffer );
            return BitConverter.ToInt64( buffer, 0 );
        }

        public static ulong NextUInt64( this Random rnd ) {
            var buffer = new byte[sizeof(long)];
            rnd.NextBytes( buffer );
            return BitConverter.ToUInt64( buffer, 0 );
        }
    }
}
