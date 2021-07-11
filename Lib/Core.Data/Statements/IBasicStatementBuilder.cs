using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public interface IBasicStatementBuilder {
		IDatabaseConnector Database { get; set; }
		IEnumerable<IParameter> Parameters { get; }
	}
}
