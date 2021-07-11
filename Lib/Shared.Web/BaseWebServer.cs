using System;

using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.HostFiltering;
using Newtonsoft.Json.Serialization;

namespace Shared.Web {
	public abstract class BaseWebServer {
		public static IWebHost Build( Type serverType, IConfiguration configuration ) {
			try {
				if( serverType.FullName == null )
					throw new ArgumentNullException( nameof( serverType.FullName ) );

				var ffConfig = configuration.GetSection( "Firefall" ).Get<Config.Firefall>();
				var hostConfig = ffConfig.WebHosts.Single( kvp => kvp.Key == serverType.FullName.Replace( ".WebServer", "" ) ).Value;

				var h = ffConfig.MainHost;

				if( hostConfig.Subdomain != null )
					h = hostConfig.Subdomain + "." + h;

				Log.Information( $"Starting web host {serverType.FullName}" );
				return WebHost.CreateDefaultBuilder()
						.UseConfiguration( configuration )
						.UseSetting( "HostName", h )
						.UseSetting( "HostType", serverType.FullName.Replace( ".WebServer", "" ) )
						.UseSerilog()
						.UseKestrel( ( builder, serverOpts ) => {
							serverOpts.ConfigureHttpsDefaults( opts => {
								opts.SslProtocols = System.Security.Authentication.SslProtocols.Tls |    // Required by FF itself *sigh*, completely insecure
													System.Security.Authentication.SslProtocols.Tls12 |
													System.Security.Authentication.SslProtocols.Tls13;
							} );

							var configuration = serverOpts.ApplicationServices.GetRequiredService<IConfiguration>();
							var environment = serverOpts.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

							foreach( var kvp in hostConfig.Endpoints ) {
								var config = kvp.Value;
								var port = config.Port ?? (config.Scheme == "https" ? 443 : 80);

								var ipAddresses = new List<IPAddress>();
								if( h == "localhost" ) {
									ipAddresses.Add( IPAddress.Loopback );
								} else if( IPAddress.TryParse( h, out var address ) ) {
									ipAddresses.Add( address );
								} else {
									ipAddresses.Add( IPAddress.Any );
								}

								foreach( var address in ipAddresses ) {
									serverOpts.Listen( address, port,
										listenOptions => {
											if( config.Scheme == "https" ) {
												var certificate = LoadCertificate( ffConfig.MainHost, config, environment );
												_ = listenOptions.UseHttps( certificate );
											}
										} );
								}
							}
						} )
					   .UseStartup( serverType )
					   //.UseUrls( h )
					   .Build();
			} catch( Exception ex ) {
				Log.Fatal( ex, "Host terminated unexpectedly" );

				return null;
			}
		}

		public static IWebHost Build<T>( IConfiguration configuration ) where T : BaseWebServer {
			return Build( typeof( T ), configuration );
		}

		public IConfiguration Configuration { get; private set; }

		public BaseWebServer( IConfiguration configuration ) {
			Configuration = configuration;
		}

		public void ConfigureServices( IServiceCollection services ) {
			_ = services.AddControllers()
					.AddNewtonsoftJson( opts => {
						opts.SerializerSettings.ContractResolver = new DefaultContractResolver {
							NamingStrategy = null
						};
					} );

			var ffConfig = Configuration.GetSection( "Firefall" ).Get<Config.Firefall>();
			_ = services.AddSingleton( ffConfig );
			_ = services.AddSingleton<Data.IFirefallDatabase>( new Data.FirefallDatabase( ffConfig.Databases[ffConfig.WebHosts[Configuration.GetValue<string>( "HostType" )].ConnectionString] ) );
			_ = services.Configure<HostFilteringOptions>( options => options.AllowedHosts = new[] { Configuration.GetValue<string>( "HostName" ) } );

			ConfigureChildServices( services );
		}

		protected virtual void ConfigureChildServices( IServiceCollection services ) { }

		public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {
			if( env.IsDevelopment() )
				_ = app.UseDeveloperExceptionPage();

			_ = app.UseHttpsRedirection()
				.UseSerilogRequestLogging()
				.UseRouting()
				.UseEndpoints( endpoints => { _ = endpoints.MapControllers(); } );

			ConfigureChild( app, env );
		}

		protected virtual void ConfigureChild( IApplicationBuilder app, IWebHostEnvironment env ) { }

		private static X509Certificate2 LoadCertificate( string hostName, Config.EndpointConfiguration config, IWebHostEnvironment environment ) {
			if( config.StoreName != null && config.StoreLocation != null ) {
				using( var store = new X509Store( config.StoreName, Enum.Parse<StoreLocation>( config.StoreLocation ) ) ) {
					store.Open( OpenFlags.ReadOnly );
					var certificate = store.Certificates.Find(
						X509FindType.FindBySubjectName,
						hostName,
						validOnly: !environment.IsDevelopment() );

					if( certificate.Count == 0 )
						throw new InvalidOperationException( $"Certificate not found for {hostName}." );

					return certificate[0];
				}
			}

			if( config.FilePath != null && config.CertPassword != null ) {
				return new X509Certificate2( config.FilePath, config.CertPassword );
			}

			throw new InvalidOperationException( "No valid certificate configuration found for the current endpoint." );
		}
	}
}
