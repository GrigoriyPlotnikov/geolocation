using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.IO;
using System.Reflection;
using NLog.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace GeoData
{
    public class Program
    {
        public const string ConfigFolderPath = "Configs";
        public static async Task Main(string[] args)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation($"The service {assemblyName.Name} is started (Version {assemblyName.Version})");

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(root);

            return Host.CreateDefaultBuilder(args)
                .UseContentRoot(root)
                .ConfigureAppConfiguration((context, config) => config
                    .AddJsonFile(Path.Combine(ConfigFolderPath, "appsettings.json"), false))
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    NLogBuilder.ConfigureNLog(Path.Combine(ConfigFolderPath, "nlog.config"));
                })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
