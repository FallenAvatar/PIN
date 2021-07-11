using System;
using System.Collections.Generic;
using System.Text;

using Core.Data;

namespace Shared.Data {
	public class FirefallDatabase : IFirefallDatabase {
		public IDatabaseConnector Connector { get; init; }

		public ITable<Account> Accounts { get; init; }

		public FirefallDatabase( IDBConfig dbConfig ) {
			Connector = DatabaseConnectorFactory.Get( dbConfig );

			Accounts = Connector.GetTable<Account>( "accounts" );
		}
	}
}
