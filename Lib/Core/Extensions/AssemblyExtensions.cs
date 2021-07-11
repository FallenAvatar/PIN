using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions {
	public static class AssemblyExtensions {
		public static IEnumerable<Type> FindTypesByBaseClass<T>( this Assembly asm ) =>
			(from t in asm.GetExportedTypes()
			 where t.IsAssignableTo( typeof( T ) )
			 select t);
	}
}
