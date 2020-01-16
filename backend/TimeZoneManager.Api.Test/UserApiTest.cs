using System.Net;
using TimeZoneManager.Api.Test.GeneratedApiClients;
using TimeZoneManager.Authorization;
using TimeZoneManager.Exceptions;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    public class UserApiTest : BaseApiTest
    {
        private readonly IUserClient _userClient;

        public UserApiTest(TimeZoneManagerTestFixture fixture)
            : base(fixture)
        {
            _userClient = new UserClient(fixture.GetClient(AdminPermissions.AdminPermissionName));
        }

        [Fact]
        public void Create()
        {
            Assert.NotNull(CreateUser());
        }

        [Fact]
        public void Register()
        {
            var user = new FullUserDto()
            {
                Username = "test",
                Password = "testpwd"
            };

            var result = _userClient.Register(user);
            Assert.NotNull(result);

            var loaded = _userClient.Load(result.Key);
            Assert.NotNull(loaded);
        }

        [Fact]
        public void Load()
        {
            var user = CreateUser();
            var result = _userClient.Load(user.Key);
            Assert.NotNull(result);
        }

        [Fact]
        public void LoadAll()
        {
            CreateUser();
            CreateUser();

            var result = _userClient.LoadAll();
            Assert.True(result.Count >= 2);
        }

        [Fact]
        public void FindByName()
        {
            var user = CreateUser();
            CreateUser();

            var result = _userClient.FindByName(user.Username);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Update()
        {
            var user = CreateUser();
            var result = _userClient.Update(user);
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete()
        {
            var user = CreateUser();
            _userClient.Delete(user.Key);

            var result = Assert.Throws<SwaggerException>(() => _userClient.Load(user.Key));
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Contains(typeof(EntityDoesNotExistException).FullName, result.Response);
        }

        private UserDto CreateUser()
        {
            var result = _userClient.Create(new FullUserDto
            {
                Username = NewGuid(),
                Password = NewGuid()
            });

            Assert.NotNull(result);

            return result;
        }
    }
}
