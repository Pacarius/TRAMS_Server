using Api;
using Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMySqlDataSource($"Server={Configs.Host};User ID={Configs.User};Password={Configs.Password};Database={Configs.Database}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseRouter(routes =>
            {
                foreach(string s in Routes.RouteList.Keys){
                    routes.MapGet(s, Routes.RouteList[s]);
                }
            }).Run(async (context) =>
            {
                Console.WriteLine(context.GetEndpoint());
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}