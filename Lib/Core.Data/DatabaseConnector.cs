using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Core.Data {
	public abstract class DatabaseConnector : IDatabaseConnector, IDisposable {
		private bool disposedValue;



		protected DbTransaction CurrentTransaction { get; private set; }
		protected DbConnection Connection { get; private set; }
		public string DbName { get; private set; }
		private string ConnectionString { get; set; }



		protected DatabaseConnector( string dbName, string connString ) {
			DbName = dbName;
			ConnectionString = connString;
		}

		protected DatabaseConnector( IDBConfig config ) {
			DbName = config.DatabaseName;
			ConnectionString = config.ConnectionString;
		}




		public async Task OpenConnection( ) {
			Connection = GetDbConnection( ConnectionString );
			await Connection.OpenAsync();
		}
		public void CloseConnection( ) {
			if( Connection == null )
				throw new InvalidOperationException( "Can not close a connection that does not exist." );

			if( Connection.State != ConnectionState.Open )
				throw new InvalidOperationException( "Can not close a connection that is not open." );

			Connection.Close();
			Connection = null;
		}

		[System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining )]
		protected async Task OpenConnectionIfNeeded( ) {
			if( Connection == null || Connection.State == ConnectionState.Closed )
				await OpenConnection();
		}

		public async Task StartTransaction( IsolationLevel il = IsolationLevel.Unspecified ) {
			await OpenConnectionIfNeeded();
			CurrentTransaction = Connection.BeginTransaction( il );
		}

		public void CommitTransaction( ) {
			if( CurrentTransaction == null )
				throw new InvalidOperationException( "Can not commit a transaction that has not been started." );

			CurrentTransaction.Commit();
			CurrentTransaction = null;
		}
		public void RollbackTransaction( ) {
			if( CurrentTransaction == null )
				throw new InvalidOperationException( "Can not commit a transaction that has not been started." );

			CurrentTransaction.Rollback();
			CurrentTransaction = null;
		}

		public ITable<T> GetTable<T>( string name ) where T : IRowView {
			return new Table<T>( this, name );
		}

		protected abstract DbConnection GetDbConnection( string cs );

		public async Task<IEnumerable<T>> ExecuteQuery<T>( string sql, IEnumerable<IParameter> parameters = null ) where T : IRowView {
			var records = await ExecuteQuery( sql, parameters );
			var ret = new List<T>();

			foreach( var r in records ) {
				ret.Add( MagicManager.GetActiveRecord<T>( this, r ) );
			}

			return ret;
		}
		public async Task<IEnumerable<IDataRow>> ExecuteQuery( string sql, IEnumerable<IParameter> parameters = null ) {
			var cmd = await BuildCommand( sql, parameters );
			var rdr = await cmd.ExecuteReaderAsync();
			var ret = new List<IDataRow>();

			while( await rdr.ReadAsync() ) {
				ret.Add( new DataRow( rdr ) );
			}

			rdr.Close();

			return ret;
		}
		public async Task<int> ExecuteNonQuery( string sql, IEnumerable<IParameter> parameters = null ) {
			var cmd = await BuildCommand( sql, parameters );

			return await cmd.ExecuteNonQueryAsync();
		}
		public async Task<T> ExecuteScalar<T>( string sql, IEnumerable<IParameter> parameters = null ) {
			var cmd = await BuildCommand( sql, parameters );
			var ret = await cmd.ExecuteScalarAsync();

			return ret == DBNull.Value ? default : (T)ret;
		}

		[System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining )]
		protected async Task<DbCommand> BuildCommand( string sql, IEnumerable<IParameter>? parameters = null ) {
			await OpenConnectionIfNeeded();

			var cmd = Connection.CreateCommand();
			cmd.Transaction = CurrentTransaction;

			if( parameters != null ) {
				foreach( var p in parameters ) {
					_ = cmd.Parameters.Add( GetParameter( p ) );
				}
			}

			cmd.CommandText = sql;
			cmd.CommandType = CommandType.Text;

			return cmd;
		}

		public abstract string Delim( string val, DelimType dt );

		public abstract IDataParameter GetParameter( string name, object? val );

		protected virtual void Dispose( bool disposing ) {
			if( !disposedValue ) {
				if( disposing ) {
					if( CurrentTransaction != null ) {
						CurrentTransaction.Dispose();
						CurrentTransaction = null;
					}

					if( Connection != null ) {
						if( Connection.State != ConnectionState.Closed )
							Connection.Close();

						Connection.Dispose();
						Connection = null;
					}
				}

				disposedValue = true;
			}
		}

		public void Dispose( ) {
			Dispose( disposing: true );
			GC.SuppressFinalize( this );
		}
	}
}
