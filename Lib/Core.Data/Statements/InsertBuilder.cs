

namespace Core.Data.Statements {
	public class InsertBuilder : IInsertBuilder {
		public IDatabaseConnector Database { get; set; }
		public string Table { get; set; }

		public InsertBuilder( IDatabaseConnector dbConn, string tblName ) { Database = dbConn; Table = tblName; }
	}
}
