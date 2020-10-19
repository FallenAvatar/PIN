using System.Threading.Tasks;

namespace WebHost.OperatorApi.Capability {
	public interface ICapabilityRepository {
		Task<HostInformation> GetHostInformationAsync( string environment, int build, string host );
		Task<ProductInformation> GetProductInformationAsync( string productName );
	}
}
