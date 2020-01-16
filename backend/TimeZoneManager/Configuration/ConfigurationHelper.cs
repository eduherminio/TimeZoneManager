using Microsoft.Extensions.Configuration;
using System.IO;
using TimeZoneManager.Constants;

namespace TimeZoneManager.Configuration
{
    public static class ConfigurationHelper
    {
        public static IConfiguration GetConfiguration(bool reloadConfigOnChange = false)
        {
            return GetConfigurationFromFile(AppSettingsKeys.ConfigurationFile, reloadConfigOnChange);
        }

        public static IConfiguration GetConfigurationFromFile(string fileName)
        {
            return GetConfigurationFromFile(fileName, false);
        }

        public static IConfiguration GetConfigurationFromFile(string fileName, bool reloadConfigOnChange)
        {
            return string.IsNullOrWhiteSpace(fileName)
                ? GetConfiguration(reloadConfigOnChange)
                : new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(fileName, optional: false, reloadOnChange: reloadConfigOnChange)
#if DEBUG
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: reloadConfigOnChange)
#endif
                    .AddEnvironmentVariables()
                .Build();
        }
    }
}
