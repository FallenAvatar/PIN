using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public interface IFilterableStatement {
		IEnumerable<IParameter> Conditions { get; }

		void Where( string field, object? val );
		void Where( Selector field, object? val );
		void Where( string field, IParameter p );
		void Where( Selector field, IParameter p );
	}
}
