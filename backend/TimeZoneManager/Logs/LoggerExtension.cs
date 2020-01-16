using Microsoft.Extensions.Logging;

namespace TimeZoneManager.Logs
{
    public static class LoggerExtension
    {
        public static void Log(this ILogger<LogAttribute> logger, LogLevel logLevel, string message, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                    logger.LogTrace(message, args);
                    break;
                case LogLevel.Debug:
                    logger.LogDebug(message, args);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(message, args);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(message, args);
                    break;
                case LogLevel.Error:
                    logger.LogError(message, args);
                    break;
                case LogLevel.Critical:
                    logger.LogCritical(message, args);
                    break;
            }
        }
    }
}
