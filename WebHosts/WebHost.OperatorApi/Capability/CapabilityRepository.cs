using System.Threading.Tasks;

using WebHost.OperatorApi.Exceptions;

namespace WebHost.OperatorApi.Capability {
	public class CapabilityRepository : ICapabilityRepository {
		public async Task<HostInformation> GetHostInformationAsync( string environment, int build, string host ) {
			return await Task.FromResult(new HostInformation {
				FrontendHost = $"https://{host}:44399",
				StoreHost = $"https://{host}:44399",
				ChatServer = $"https://{host}:44307",
				ReplayHost = $"https://{host}:44399/{environment}-{build}",
				WebHost = $"https://{host}:44399",
				MarketHost = $"https://{host}:44399",
				IngameHost = $"https://{host}:44303",
				ClientapiHost = $"https://{host}:44302",
				WebAssetHost = $"https://{host}:44399",
				WebAccountsHost = $"https://{host}:44399",
				RhsigscanHost = $"https://{host}:44399"
			});
		}

		public async Task<ProductInformation> GetProductInformationAsync( string productName ) {
			if( productName != "Firefall_Beta" ) {
				throw new NotFoundException($"Product '{productName}' is unknown");
			}

			return await Task.FromResult(new ProductInformation {
				Build = "beta-1973",
				Environment = "production",
				Region = "NA",
				PatchLevel = 0
			});
		}
	}
}