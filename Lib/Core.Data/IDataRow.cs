using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public interface IDataRow {
		int FieldCount { get; }
		IList<string> FieldNames { get; }
		object? this[string name] { get; }
	}
}
