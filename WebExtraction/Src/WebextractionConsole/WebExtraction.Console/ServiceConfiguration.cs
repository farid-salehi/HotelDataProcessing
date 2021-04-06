using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using WebExtraction.Application.Implementations;
using WebExtraction.Application.Interfaces;
using WebExtraction.Application.Settings;

namespace WebExtraction.Console
{
    public static class ServiceConfiguration
    {
        internal static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));
            services.AddLogging(builder =>
            {
                builder.AddNLog("nlog.config");
            });
            services.AddTransient<IHotelService, HotelService>();
            services.AddTransient<IHtmlFileService, HtmlFileService>();
        }
    }
}
