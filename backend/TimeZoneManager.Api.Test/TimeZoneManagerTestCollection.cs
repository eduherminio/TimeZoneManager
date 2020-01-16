using Xunit;

namespace TimeZoneManager.Api.Test
{
    /// <summary>
    /// Test group definition class
    /// </summary>
    [CollectionDefinition(Name)]
    public class TimeZoneManagerTestCollection : ICollectionFixture<TimeZoneManagerTestFixture>
    {
        internal const string Name = "TimeZoneManagerTestCollection";
    }
}
