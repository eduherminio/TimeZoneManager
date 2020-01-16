using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace TimeZoneManager.Api.Logs
{
    public static class LogsExtensions
    {
        private const string NLogConfigFile = "nlog.config";

        static LogsExtensions()
        {
            NLogBuilder.ConfigureNLog(NLogConfigFile);
        }

        public static IWebHostBuilder UseCustomLogs(this IWebHostBuilder builder)
        {
            return builder.ConfigureLogging((_, logging) =>
            {
                logging.ClearProviders();
                //logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();
        }

        public static NLog.Logger GetLogger()
        {
            return NLog.LogManager.GetCurrentClassLogger();
        }
    }
}
