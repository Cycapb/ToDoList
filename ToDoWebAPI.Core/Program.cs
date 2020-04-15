using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace ToDoWebAPI.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((hostBuilderContext, configBuilder) =>
            {
                var env = hostBuilderContext.HostingEnvironment;
                configBuilder
                    .AddJsonFile($"appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                configBuilder.AddEnvironmentVariables();

                if (args != null)
                {
                    configBuilder.AddCommandLine(args);
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                webBuilder.UseKestrel();
                webBuilder.UseIISIntegration();
                webBuilder.UseStartup<Startup>();
                webBuilder.UseDefaultServiceProvider(configure => configure.ValidateScopes = false);
            });
    }
}
