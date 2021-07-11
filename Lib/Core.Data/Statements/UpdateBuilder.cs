

namespace Core.Data.Statements {
	public class UpdateBuilder : IUpdateBuilder {
		public IDatabaseConnector Database { get; set; }
		public string Table { get; set; }

		public UpdateBuilder( IDatabaseConnector dbConn, string tblName ) { Database = dbConn; Table = tblName; }
	}
}
