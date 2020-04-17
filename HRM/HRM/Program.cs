using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Services.MongoDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            
            //scope
            using (var scope = webHost.Services.CreateScope())
            {
                var mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
                await mongoDbService.ValidateDb();
            }

            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:5000");
                    webBuilder.UseKestrel();
                });
    }
}
