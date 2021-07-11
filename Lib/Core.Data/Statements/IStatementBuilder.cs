using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public interface IStatementBuilder {
		IDatabaseConnector Database { get; set; }
		ISelectBuilder Select( );
		IInsertBuilder Insert( string table );
		IInsertBuilder Insert( ITable table ) { return Insert( table.Name ); }
		IUpdateBuilder Update( string table );
		IUpdateBuilder Update( ITable table ) { return Update( table.Name ); }
		IDeleteBuilder Delete( string table );
		IDeleteBuilder Delete( ITable table ) { return Delete( table.Name ); }
	}
}
