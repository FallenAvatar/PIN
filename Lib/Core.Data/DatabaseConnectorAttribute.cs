using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class DatabaseConnectorAttribute : Attribute {
		public string DBType { get; init; }

		public DatabaseConnectorAttribute( string name ) { DBType = name; }
	}
}
