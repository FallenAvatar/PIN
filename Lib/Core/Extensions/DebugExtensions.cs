using System.Threading.Tasks;
using System.Diagnostics;

namespace Core.Extensions {
	public static class DebugExtensions {
		public static async Task AwaitDebugger( ) {
			await Task.Run( function: async ( ) => {
				while( !Debugger.IsAttached )
					await Task.Delay( 100 );
			} );
		}
	}
}
