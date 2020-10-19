using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using WebHost.OperatorApi.Capability;

namespace WebHost.OperatorApi.Controllers {
	[ApiController]
	public class CapabilityController : ControllerBase {
		private readonly ICapabilityRepository _capabilityRepository;
		private readonly ILogger<CapabilityController> _logger;
		private readonly Shared.Web.Config.Firefall _config;

		public CapabilityController( ILogger<CapabilityController> logger,
									ICapabilityRepository capabilityRepository, Shared.Web.Config.Firefall configuration ) {
			_logger = logger;
			_capabilityRepository = capabilityRepository;
			_config = configuration;
		}

		[HttpGet]
		[Route("check")]
		public async Task<HostInformation> CheckAsync( string environment, int build ) {
			return await _capabilityRepository.GetHostInformationAsync(environment, build, _config.MainHost);
		}

		[HttpGet]
		[Route("api/v1/products/{productName}")]
		public async Task<ProductInformation> Products( string productName ) {
			return await _capabilityRepository.GetProductInformationAsync(productName);
		}
	}
}