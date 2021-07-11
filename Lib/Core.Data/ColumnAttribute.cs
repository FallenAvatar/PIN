using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class ColumnAttribute : Attribute {
		public string? ColumnName { get; init; }

		public ColumnAttribute( string? name = null ) {
			ColumnName = name;
		}
	}
}
