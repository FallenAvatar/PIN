using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Common.Extensions {
	public static class Type {
		public static T GetAttribute<T>( this System.Type type, bool inherit = false ) where T : Attribute {
			return type.GetCustomAttributes( typeof( T ), inherit ).FirstOrDefault() as T;
		}
	}
}
