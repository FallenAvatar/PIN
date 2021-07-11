using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data;

namespace Shared.Data {
	public abstract class BaseRowView : ActiveRecord {
		[PrimaryKey]
		[Column( "id" )]
		public ulong? ID { get; protected set; }
	}
}
