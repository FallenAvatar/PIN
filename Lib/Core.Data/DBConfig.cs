using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class DBConfig : IDBConfig {
		public string DatabaseName { get; set; }
		public string DatabaseType { get; set; }
		public string ConnectionString { get; set; }
	}
}
