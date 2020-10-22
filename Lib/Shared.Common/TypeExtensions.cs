using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Common {
    public static class TypeExtensions {
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute {
            return type.GetCustomAttributes( typeof( T ), false ).FirstOrDefault() as T;
        }
    }
}
