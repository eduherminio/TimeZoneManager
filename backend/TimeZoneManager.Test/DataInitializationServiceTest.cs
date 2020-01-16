using TimeZoneManager.Constants;
using TimeZoneManager.Services;
using Xunit;

namespace TimeZoneManager.Test
{
    public class DataInitializationServiceTest : BaseUnitTest
    {
        private IDataInitializationService _service;

        public DataInitializationServiceTest()
        {
            RenewServices();
        }

        protected override void RenewServices()
        {
            _service = GetService<IDataInitializationService>();
        }

        [Fact]
        public void Initialize()
        {
            NewContext();
            _service.Initialize();

            NewContext();
            var permissions = GetService<IPermissionService>().LoadAll();
            var roles = GetService<IRoleService>().LoadAll();
            var users = GetService<IUserService>().LoadAll();

            Assert.Equal(DefaultPermissions.AllPermissionsList.Count, permissions.Count);
            Assert.Equal(DefaultRoles.AllRoleList.Count, roles.Count);
            Assert.Equal(DefaultUsers.AllUserList.Count, users.Count);
        }
    }
}
