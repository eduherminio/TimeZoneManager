using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using TimeZoneManager.Api.Test.Utils;
using TimeZoneManager.Authentication;
using TimeZoneManager.Orm;

namespace TimeZoneManager.Api.Test
{
    public class TimeZoneManagerTestFixture : IDisposable
    {
        public readonly IHost Host;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = new JwtTokenGenerator();

        private const string DefaultUser = "admin";
        private const string DefaultPassword = "admin";

        public TimeZoneManagerTestFixture()
        {
            Host = IntegrationTestUtils.CreateTestServer<Startup>();
        }

        public HttpClient GetNonAuthenticatedClient()
        {
            return IntegrationTestUtils.GetHttpClient(Host);
        }

        public HttpClient GetAuthenticatedClient()
        {
            return GetClient(DefaultUser, DefaultPassword);
        }

        public HttpClient GetClient(string username, string password)
        {
            return IntegrationTestUtils.GetAuthHttpClient(Host, Host, username, password);
        }

        public HttpClient GetClient(params string[] permissions)
        {
            HttpClient client = Host.GetTestClient();

            JwtTokenPayload payload = new JwtTokenPayload()
            {
                Username = "admin",
                Permissions = permissions
            };
            string token = new JwtManager(_jwtTokenGenerator).GenerateToken(payload);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public TService GetService<TService>()
        {
            return Host.Services.GetRequiredService<TService>();
        }

        public static Uri CreateUri(string str)
        {
            return new Uri(str, UriKind.Relative);
        }

        #region IDisposable implementation

        protected virtual void Dispose(bool disposing)
        {
            var context = GetService<DatabaseContext>();
            context.Database.EnsureDeleted();
            Host.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
