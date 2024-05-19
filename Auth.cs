using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services.Builder.Configuration;

namespace PnPCoreLargeLists
{
    public class Auth
    {
        public static async Task<IHost> Initialize()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddPnPCore();
                    services.Configure<PnPCoreOptions>(hostingContext.Configuration.GetSection("PnPCore"));
                    services.AddPnPCoreAuthentication();
                    services.Configure<PnPCoreAuthenticationOptions>(hostingContext.Configuration.GetSection("PnPCore"));
                })
                .UseConsoleLifetime()
                .Build();

            await host.StartAsync();

            return host;
        }
    }
}
