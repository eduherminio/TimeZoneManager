using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TimeZoneManager.Api.Test.GeneratedApiClients;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    public class AuthorizationTest : BaseApiTest
    {
        public AuthorizationTest(TimeZoneManagerTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public void ShouldNotAccessMethodWithAuthorizationAttribute()
        {
            var httpClient = _fixture.GetClient(new[] { nameof(PermissionName.UserRead) });
            IUserClient client = new UserClient(httpClient);

            // Act
            var result = Assert.Throws<SwaggerException>(() => client.Create(new FullUserDto() { Username = ".", Password = "." }));

            // Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Contains("Unauthorized access", result.Response);
        }

        [Fact]
        public void AccessMethodWithAuthorizationAttribute()
        {
            var httpClient = _fixture.GetClient(new[] { nameof(PermissionName.UserCreate) });
            IUserClient client = new UserClient(httpClient);

            // Act and assert
            var result = client.Create(new FullUserDto() { Username = ".", Password = "." });

            // Assert
            Assert.NotNull(result);
        }
    }
}
