

namespace Core.Data.Statements {
	public class DeleteBuilder : IDeleteBuilder {
		public IDatabaseConnector Database { get; set; }
		public string Table { get; set; }

		public DeleteBuilder( IDatabaseConnector dbConn, string tblName ) { Database = dbConn; Table = tblName; }
	}
}
