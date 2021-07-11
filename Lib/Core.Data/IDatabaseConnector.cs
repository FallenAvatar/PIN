using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using Core.Data.Statements;

namespace Core.Data {
	public enum DelimType {
		None = 0,
		Database = 1,
		Schema = 2,
		Table = 3,
		Column = 4,
		Parameter = 5,
		String = 6,
	}

	public interface IDatabaseConnector {
		string DbName { get; }

		Task OpenConnection( );
		void CloseConnection( );

		Task StartTransaction( IsolationLevel il = IsolationLevel.Unspecified );
		void CommitTransaction( );
		void RollbackTransaction( );

		ITable<T> GetTable<T>( string name ) where T : IRowView;

		Task<IEnumerable<T>> ExecuteQuery<T>( string sql, IEnumerable<IParameter> parameters = null ) where T : IRowView;
		Task<IEnumerable<IDataRow>> ExecuteQuery( string sql, IEnumerable<IParameter> parameters = null );
		Task<int> ExecuteNonQuery( string sql, IEnumerable<IParameter> parameters = null );
		Task<T> ExecuteScalar<T>( string sql, IEnumerable<IParameter> parameters = null );

		string Delim( string val, DelimType dt );
		string DelimDatabase( string val ) { return Delim( val, DelimType.Database ); }
		string DelimSchema( string val ) { return Delim( val, DelimType.Schema ); }
		string DelimTable( string val ) { return Delim( val, DelimType.Table ); }
		string DelimColumn( string val ) { return Delim( val, DelimType.Column ); }
		string DelimParameter( string val ) { return Delim( val, DelimType.Parameter ); }
		string DelimString( string val ) { return Delim( val, DelimType.String ); }

		IDataParameter GetParameter( string name, object? val );
		IDataParameter GetParameter( IParameter p ) { return GetParameter( p.Name, p.Value ); }
		IStatementBuilder GetStatementBuilder( ) { return new StatementBuilder( this ); }
	}
}
