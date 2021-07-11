using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Core.Data {
	public interface IRowView {
		IDatabaseConnector Database { get; set; }
		object this[string column] { get; }
		void Load( IDataRow row );
	}
}
