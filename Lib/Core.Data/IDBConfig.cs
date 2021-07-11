using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public interface IDBConfig {
		string DatabaseName { get; }
		string DatabaseType { get; }
		string ConnectionString { get; }
	}
}
