using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public class Server
    {
        public IWebHost Host { get; private set; }
        public Server()
        {
            Host = CreateWebHostBuilder([]).Build();
            Host.RunAsync();
        }
        IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddRouting())
                .UseKestrel(options =>
                {
                    options.ListenLocalhost(
                        Program.configHelper != null ?Program.configHelper.Configs.tramsConfig.TRAMSConfig.EspPort : 42069
                        , builder => builder.UseConnectionHandler<EspHandler>());
                    options.ListenLocalhost(
                        Program.configHelper != null ?Program.configHelper.Configs.tramsConfig.TRAMSConfig.ApiPort : 9696
                        );
                }).UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "True")
                .UseStartup<Startup>();
        public async Task Stop() => await Host.StopAsync();

    }

}
