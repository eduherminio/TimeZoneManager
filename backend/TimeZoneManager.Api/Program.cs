using System;
using AspectCore.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TimeZoneManager.Api.Logs;
using TimeZoneManager.Configuration;
using TimeZoneManager.Constants;

namespace TimeZoneManager.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogsExtensions.GetLogger();

            try
            {
                logger.Debug("Starting TimeZoneManager.Api");

                var host = BuildHostBuilder(args, GetConfiguration(), AppSettingsKeys.UrlConfigurationKey);

                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e, $"Stopped program because of exception: {e.Message}");
                throw;
            }
        }

        public static IHost BuildHostBuilder(string[] args, IConfiguration configuration, string urlKey)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseServiceContext()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(configuration[urlKey]);
                    webBuilder.UseCustomLogs();
                })
                .Build();
        }

        private static IConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration();
        }
    }
}
