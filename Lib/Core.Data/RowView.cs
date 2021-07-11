using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Core.Extensions;

namespace Core.Data {
	public abstract class RowView : IRowView {
		public IDatabaseConnector Database { get; set; }

		protected Dictionary<string, object?> data;
		public object? this[string column] { get { return data[column]; } }

		protected RowView( ) { }

		public virtual void Load( IDataRow row ) {
			data = new Dictionary<string, object?>();

			for( var i = 0; i < row.FieldCount; i++ ) {
				var n = row.FieldNames[i];
				object? v;

				if( row[n] == DBNull.Value )
					v = null;
				else
					v = row[n];

				data.Add( n, v );
			}
		}
	}
}
