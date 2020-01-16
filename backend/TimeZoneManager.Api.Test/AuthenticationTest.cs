using System.Net;
using TimeZoneManager.Api.Test.GeneratedApiClients;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    public class AuthenticationTest : BaseApiTest
    {
        public AuthenticationTest(TimeZoneManagerTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void NoJwtTokenRequiredMethod()
        {
            // Arrange
            var httpClient = _fixture.GetNonAuthenticatedClient();
            ILoginClient client = new LoginClient(httpClient);

            // Act
            var result = Assert.Throws<SwaggerException>(() => client.Index("", ""));

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Contains("Unauthorized access", result.Response);
        }

        [Fact]
        public void ShouldNotAccessMethodWithJwtTokenRequired()
        {
            var httpClient = _fixture.GetNonAuthenticatedClient();
            ILoginClient client = new LoginClient(httpClient);

            // Act
            var result = Assert.Throws<SwaggerException>(() => client.RenewToken());

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Contains("Unauthorized access", result.Response);
        }
    }
}
