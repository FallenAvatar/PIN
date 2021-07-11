using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class Table<T> : ITable<T> where T : IRowView {
		public string Name { get; init; }
		public IDatabaseConnector DBConnector { get; }
		public int RowCount { get; }

		public Table( IDatabaseConnector dbConn, string name ) {
			Name = name;
			DBConnector = dbConn;
		}

		public S Scalar<S>( ) where S : struct {
			return default( S );
		}

		public async Task<IEnumerable<T>> Select( string? where = null ) {
			var sql = "SELECT * FROM " + DBConnector.DelimTable( Name );

			if( where != null ) {
				throw new NotImplementedException();
			}

			return await DBConnector.ExecuteQuery<T>( sql );
		}
		public int Insert( T row ) {
			return 0;
		}
		public int Update( T row ) {
			return 0;
		}
		public int Delete( T row ) {
			return 0;
		}
	}
}
