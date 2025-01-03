using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI
{
    public class Program
    {
        public static ILogger<Program> Logger { get; private set; }

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // Set up the logger
            Logger = host.Services.GetService(typeof(ILogger<Program>)) as ILogger<Program>;
            // Example logging
            Logger?.LogInformation("Application has started.");

            using (var scope = host.Services.CreateScope())
            {
                SeedData.Initialize(scope.ServiceProvider);
            }

            // Run the host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    // Add logging providers (console, debug, etc.)
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
