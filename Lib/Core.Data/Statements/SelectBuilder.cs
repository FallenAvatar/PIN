

namespace Core.Data.Statements {
	public class SelectBuilder : ISelectBuilder {
		public IDatabaseConnector Database { get; set; }

		public SelectBuilder( IDatabaseConnector dbConn ) { Database = dbConn; }
	}
}
