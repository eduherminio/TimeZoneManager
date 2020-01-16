using System.Linq;
using TimeZoneManager.Api.Test.GeneratedApiClients;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    public class TimeZoneApiTest : BaseApiTest
    {
        private readonly ITimeZoneClient _timeZoneClient;

        public TimeZoneApiTest(TimeZoneManagerTestFixture fixture)
            : base(fixture)
        {
            _timeZoneClient = new TimeZoneClient(fixture.GetClient(
                nameof(PermissionName.TimeZoneAdmin),
                nameof(PermissionName.TimeZoneCreate),
                nameof(PermissionName.TimeZoneDelete),
                nameof(PermissionName.TimeZoneRead),
                nameof(PermissionName.TimeZoneUpdate)));
        }

        [Fact]
        public void Create()
        {
            CreateTimeZone();
        }

        [Fact]
        public void Load()
        {
            var timeZone = CreateTimeZone();
            var result = _timeZoneClient.FindByName(timeZone.Name).Single();
            Assert.NotNull(result);
        }

        [Fact]
        public void LoadAll()
        {
            CreateTimeZone();
            CreateTimeZone();

            var result = _timeZoneClient.LoadAll();
            Assert.True(result.Count >= 2);
        }

        [Fact]
        public void Update()
        {
            var timeZone = CreateTimeZone();
            var result = _timeZoneClient.Update(timeZone);
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete()
        {
            var timeZone = CreateTimeZone();
            _timeZoneClient.Delete(timeZone.Key);

            Assert.Empty(_timeZoneClient.FindByName(timeZone.Key));
        }

        private TimeZoneDto CreateTimeZone()
        {
            var result = _timeZoneClient.Create(new TimeZoneDto
            {
                Name = NewGuid(),
                CityName = NewGuid(),
                GmtDifferenceInHours = -5
            });

            Assert.NotNull(result);

            return result;
        }
    }
}
