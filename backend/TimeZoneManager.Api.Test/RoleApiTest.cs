using System.Net;
using TimeZoneManager.Api.Test.GeneratedApiClients;
using TimeZoneManager.Authorization;
using TimeZoneManager.Exceptions;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    public class RoleApiTest : BaseApiTest
    {
        private readonly IRoleClient _roleClient;

        public RoleApiTest(TimeZoneManagerTestFixture fixture)
            : base(fixture)
        {
            _roleClient = new RoleClient(fixture.GetClient(nameof(GeneratedApiClients.PermissionName.RoleRead)));
        }

        [Fact]
        public void LoadAll()
        {
            var result = _roleClient.LoadAll();
            Assert.True(result.Count >= 2);
        }
    }
}
