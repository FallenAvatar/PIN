using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Shared.Web;

//using WebHost.OperatorApi.Capability;
using WebHost.OperatorApi.Exceptions;

namespace WebHost.OperatorApi.Controllers {
	[ApiController]
	public class OperatorController : ControllerBase {
		private readonly ILogger<OperatorController> _logger;
		private readonly Shared.Web.Config.Firefall _config;
		private readonly Shared.Data.IFirefallDatabase _db;

		public OperatorController( ILogger<OperatorController> logger, Shared.Web.Config.Firefall configuration, Shared.Data.IFirefallDatabase db ) {
			_logger = logger;
			_config = configuration;
			_db = db;
		}

		[HttpGet]
		[Route( "check" )]
		[FirefallClientFilter]
		public Dictionary<string, string> CheckAsync( string environment, int build ) {
			var ret = new Dictionary<string, string>();
			var catchallName = "WebHost.CatchAll";
			var extraHosts = new[] { "frontend_host", "web_host", "web_accounts_host", "rhsigscan_host" };

			var prot = "Http";

			if( Request.Scheme == "https" )
				prot = "Https";

			foreach( var kvp in _config.WebHosts ) {
				if( kvp.Key == catchallName )
					continue;

				ret.Add( kvp.Value.Red5Name, prot.ToLower() + "://" + kvp.Value.Subdomain + "." + _config.MainHost + ":" + (kvp.Value.Endpoints[prot].Port ?? (prot == "Http" ? 80 : 443)) );
			}

			foreach( var h in extraHosts ) {
				ret.Add( h, prot.ToLower() + "://" + _config.WebHosts[catchallName].Subdomain + "." + _config.MainHost + ":" + (_config.WebHosts[catchallName].Endpoints[prot].Port ?? (prot == "Http" ? 80 : 443)) );
			}

			return ret;
		}

		[HttpGet]
		[Route( "test" )]
		public async Task<IEnumerable<Shared.Data.Account>> Test( string environment, int build ) {
			var accts = await _db.Accounts.Select();

			var mine = accts.Where( a => a.Username == "FallenAvatar" ).First();
			mine.Password = "test";
			mine.Save();

			return accts;
		}

		[HttpGet]
		[Route( "api/v1/products/{productName}" )]
		[FirefallClientFilter]
		public ActionResult<object> Products( string productName ) {
			if( productName != "Firefall_Beta" ) {
				return NotFound();
			}

			return new {
				Build = "beta-1973",
				Environment = "production",
				Region = "NA",
				PatchLevel = 0
			};
		}
	}
}