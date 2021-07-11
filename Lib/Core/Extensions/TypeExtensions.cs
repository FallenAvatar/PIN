using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions {
	public static class TypeExtensions {
		public static IEnumerable<Type> FindTypesByBaseClass<T>( ) =>
			(from a in (from a in AppDomain.CurrentDomain.GetAssemblies()
						where a.GetReferencedAssemblies().Contains( Assembly.GetExecutingAssembly().GetName() )
						select a)
			 from t in a.GetExportedTypes()
			 where t.IsAssignableTo( typeof( T ) )
			 select t);

		public static T? GetAttribute<T>( this Type type, bool inherit = false ) where T : Attribute {
			return type.GetCustomAttributes( typeof( T ), inherit ).FirstOrDefault() as T;
		}

		public static IEnumerable<MemberInfo> GetMembersByAttribute<T>( this Type type, bool inherit = false ) where T : Attribute {
			return type.GetMembers().Where( ( m ) => m.GetCustomAttributes( typeof( T ), inherit ).FirstOrDefault() is not null );
		}

		public static IEnumerable<MemberInfo> GetMembersByAttribute<T>( this Type type, BindingFlags b, bool inherit = false ) where T : Attribute {
			return type.GetMembers( b ).Where( ( m ) => m.GetCustomAttributes( typeof( T ), inherit ).FirstOrDefault() is not null );
		}
	}
}
