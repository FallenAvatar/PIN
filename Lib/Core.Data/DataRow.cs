using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class DataRow : IDataRow {
		public int FieldCount { get; private set; }
		public IList<string> FieldNames { get; private set; }

		protected Dictionary<string, object> data;
		public object this[string name] { get { return data[name]; } }

		public DataRow( IDataRecord row ) {
			FieldCount = row.FieldCount;
			FieldNames = new string[FieldCount];
			data = new Dictionary<string, object>( FieldCount );

			for( var i = 0; i < FieldCount; i++ ) {
				FieldNames[i] = row.GetName( i );
				data[FieldNames[i]] = row[i];
			}
		}
	}
}
