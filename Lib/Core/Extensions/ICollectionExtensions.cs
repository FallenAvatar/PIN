using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions {
	public static class ICollectionExtensions {
		public static void AddAll<T>( this ICollection<T> col, IEnumerable<T> items ) {
			foreach( var item in items )
				col.Add( item );
		}
	}
}
