using System;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;

namespace Core.Data {
	public static class MagicManager {
		private static ConcurrentDictionary<Type, Func<IDataRow, IRowView>> converters;
		public static T GetActiveRecord<T>( IDatabaseConnector dbConn, IDataRow rec ) where T : IRowView {
			if( converters == null )
				converters = new ConcurrentDictionary<Type, Func<IDataRow, IRowView>>();

			var t = typeof( T );
			Func<IDataRow, IRowView> c;
			if( !converters.ContainsKey( t ) )
				c = converters.AddOrUpdate( t, BuildConverter<T>( dbConn ), ( t2, nc ) => nc );
			else
				c = converters[t];

			return (T)c( rec );
		}

		private static Func<IDataRow, IRowView> BuildConverter<T>( IDatabaseConnector dbConn ) {
			return BuildConverter( dbConn, typeof( T ) );
		}

		private static Func<IDataRow, IRowView> BuildConverter( IDatabaseConnector dbConn, Type t ) {
			return ( IDataRow dr ) => {
				var ret = Activator.CreateInstance( t ) as IRowView;
				ret.Database = dbConn;
				ret.Load( dr );
				return ret;
			};
		}
	}
}