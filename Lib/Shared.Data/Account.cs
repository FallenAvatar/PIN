using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Data;

using Newtonsoft.Json;

namespace Shared.Data {
	public class Account : BaseRowView {
		[Column( "username" )]
		public string Username { get; set; }
		[JsonIgnore]
		[Column( "password" )]
		public byte[] Password { get; set; }
		[Column( "created_at" )]
		public DateTime CreatedAt { get; set; }
	}
}
