using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public interface ITable {
		string Name { get; }
		int RowCount { get; }

		Task<IEnumerable<IRowView>> Select( string where = null );
		int Insert( IRowView row );
		int Update( IRowView row );
		int Delete( IRowView row );
	}
	public interface ITable<T> : ITable where T : IRowView {

		new Task<IEnumerable<T>> Select( string where = null );
		int Insert( T row );
		int Update( T row );
		int Delete( T row );
	}
}
