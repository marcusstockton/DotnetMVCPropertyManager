using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set application-wide default culture to en-GB so currency and date formatting use £ and UK formats
            var enGb = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = enGb;
            CultureInfo.DefaultThreadCurrentUICulture = enGb;

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.AddConsole();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}