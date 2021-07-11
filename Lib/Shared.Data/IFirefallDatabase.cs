using System;
using System.Collections.Generic;
using System.Text;

using Core.Data;

namespace Shared.Data {
	public interface IFirefallDatabase {
		IDatabaseConnector Connector { get; }

		ITable<Account> Accounts { get; }
	}
}
