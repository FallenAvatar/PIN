using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public interface IQueryStatementBuilder : IBasicStatementBuilder {
		Task<IEnumerable<T>> Execute<T>( ) where T : IRowView { return Database.ExecuteQuery<T>( ToString(), Parameters ); }
		Task<T> ExecuteScalar<T>( ) where T : IRowView { return Database.ExecuteScalar<T>( ToString(), Parameters ); }
	}
}
