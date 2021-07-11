using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Web.Config {
	public class WebHost {
		public Dictionary<string,EndpointConfiguration> Endpoints { get; set; } = new Dictionary<string,EndpointConfiguration>();
		public string ConnectionString { get; set; }
		public string? Subdomain { get; set; }
		public string? Red5Name { get; set; }
	}

	public class EndpointConfiguration {
		public int? Port { get; set; }
		public string Scheme { get; set; }
		public string StoreName { get; set; }
		public string StoreLocation { get; set; }
		public string FilePath { get; set; }
		public string CertPassword { get; set; }
	}
}
