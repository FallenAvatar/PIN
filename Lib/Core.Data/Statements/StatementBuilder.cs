using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public class StatementBuilder : IStatementBuilder {
		public IDatabaseConnector Database { get; set; }

		public StatementBuilder( IDatabaseConnector dbConn ) { Database = dbConn; }

		public ISelectBuilder Select( ) => new SelectBuilder( Database );
		public IInsertBuilder Insert( string table ) => new InsertBuilder( Database, table );
		public IUpdateBuilder Update( string table ) => new UpdateBuilder( Database, table );
		public IDeleteBuilder Delete( string table ) => new DeleteBuilder( Database, table );
	}
}