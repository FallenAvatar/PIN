﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Web.Config {
	public class Firefall {
		public string MainHost { get; set; } = "localhost";
		public Dictionary<string, WebHost> WebHosts { get; set; } = new Dictionary<string, WebHost>();
	}
}
