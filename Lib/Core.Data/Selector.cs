using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Extensions;

namespace Core.Data {
	public class Selector {
		public struct SelectorPart {
			public string Text { get; set; }
			public DelimType Type { get; set; }
		}
		public IDatabaseConnector Database { get; set; }
		public string? Alias { get; set; }
		public IList<SelectorPart> Parts { get; init; } = new List<SelectorPart>();

		public Selector( IDatabaseConnector dbConn, params SelectorPart[] parts ) {
			Database = dbConn;
			Parts.AddAll( parts );
		}

		public Selector( params SelectorPart[] parts ) {
			Parts.AddAll( parts );
		}

		public override string ToString( ) {
			return ToString( false );
		}

		public string ToString( bool withAlias = false ) {
			var ret = "";
			bool first = true;

			foreach( var p in Parts ) {
				if( !first )
					ret += ".";

				ret += $"{Database.Delim( p.Text, p.Type )}";
				first = false;
			}

			if( withAlias && Alias != null )
				ret += $" AS {Alias}";

			return ret;
		}
	}
}
