using System;
using Xunit;

namespace TimeZoneManager.Api.Test
{
    [Collection(TimeZoneManagerTestCollection.Name)]
    public class BaseApiTest
    {
        protected readonly TimeZoneManagerTestFixture _fixture;

        protected BaseApiTest(TimeZoneManagerTestFixture fixture)
        {
            _fixture = fixture;
        }

        protected static string NewGuid() => Guid.NewGuid().ToString();
    }
}
