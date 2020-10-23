using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Web.Config {
	public class WebHost {
		public List<EndpointConfiguration> Endpoints { get; set; } = new List<EndpointConfiguration>();
	}

    public class EndpointConfiguration {
        public int? Port { get; set; }
        public string Scheme { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public string FilePath { get; set; }
        public string Password { get; set; }
    }
}
