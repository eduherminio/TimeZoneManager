using Microsoft.EntityFrameworkCore;

namespace TimeZoneManager.Orm
{
    /// <summary>
    /// Wrapper around Microsoft.EntityFrameworkCore0.DbContext, making easier the use of DI.
    /// </summary>
    public interface IDatabaseContextContainer
    {
        DbContext Context { get; }
    }
}
