using AspectCore.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneManager.Api.Test.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using TimeZoneManager.Api.Logs;
using TimeZoneManager.Authentication;
using TimeZoneManager.Authorization;
using TimeZoneManager.Configuration;
using TimeZoneManager.Encryption;

namespace TimeZoneManager.Api.Test.Utils
{
    public static class IntegrationTestUtils
    {
        public static IHost CreateTestServer<TStartup>()
            where TStartup : class
        {
            return CreateTestServer<TStartup>(ConfigurationHelper.GetConfiguration());
        }

        public static IHost CreateTestServer<TStartup>(IConfiguration configuration)
            where TStartup : class
        {
            return CreateTestServer<TStartup>(configuration, (_) => { }, Array.Empty<string>());
        }

        public static IHost CreateTestServer<TStartup>(IConfiguration configuration, Action<IServiceCollection> configureServices, IEnumerable<string> hostingStartupAssemblies)
            where TStartup : class
        {
            return Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseTestServer();

                    webBuilder.UseConfiguration(configuration);
                    webBuilder.ConfigureServices(configureServices);
                    webBuilder.UseStartup<TStartup>();
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, string.Join(";", hostingStartupAssemblies));
                    webBuilder.UseCustomLogs();
                })
                .UseServiceContext()
                .Start();
        }

        public static HttpClient GetHttpClient(IHost host)
        {
            return host.GetTestClient();
        }

        public static HttpClient GetAuthHttpClient(IHost host, IHost authHost, string username, string password)
        {
            HttpClient client = host.GetTestClient();
            if (authHost != null)
            {
                string authToken = GetAuthToken(authHost, username, password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtConstants.Bearer, authToken);
            }
            return client;
        }

        public static HttpClient GetAdminClient(IHost host)
        {
            IJwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();
            HttpClient client = host.GetTestClient();
            JwtTokenPayload payload = new JwtTokenPayload()
            {
                Username = "admin",
                Permissions = new[] { AdminPermissions.AdminPermissionName }
            };
            string token = new JwtManager(jwtTokenGenerator).GenerateToken(payload);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtConstants.Bearer, token);

            return client;
        }

        private static string GetAuthToken(IHost authHost, string username, string password)
        {
            var httpClient = GetHttpClient(authHost);
            Uri tokenUri = new Uri($"/api/login?username={username}&password={EncryptionHelpers.GenerateHash(password)}", UriKind.Relative);
            string token = HttpHelper.Post(httpClient, tokenUri, string.Empty, out HttpStatusCode statusCode);

            if (statusCode != HttpStatusCode.OK)
            {
                throw new UnauthorizedAccessException("Error creating auth token");
            }

            return token;
        }
    }
}
