using System.Data;
using System.Data.Common;

using Core.Data;

namespace Core.Data.MySql {
	[DatabaseConnector( "MySql" )]
	public class DatabaseConnector : Data.DatabaseConnector {
		public DatabaseConnector( string dbName, string connString ) : base( dbName, connString ) { }

		public DatabaseConnector( IDBConfig config ) : base( config ) { }

		public override string Delim( string val, DelimType dt ) {
			switch( dt ) {
			case DelimType.Parameter:
				return '@' + val;
			case DelimType.String:
				return "'" + val.Replace( "'", "''" ) + "'";
			default:
				return '`' + val.Replace( "`", "``" ) + '`';
			}
		}

		protected override DbConnection GetDbConnection( string cs ) {
			return new global::MySql.Data.MySqlClient.MySqlConnection( cs );
		}

		public override IDataParameter GetParameter( string name, object? val ) {
			return new global::MySql.Data.MySqlClient.MySqlParameter( name, val );
		}
	}
}