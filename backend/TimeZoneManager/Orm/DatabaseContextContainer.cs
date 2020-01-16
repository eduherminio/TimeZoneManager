using Microsoft.EntityFrameworkCore;

namespace TimeZoneManager.Orm
{
    public class DatabaseContextContainer<TContext> : IDatabaseContextContainer
        where TContext : DbContext
    {
        public DbContext Context { get; private set; }

        public DatabaseContextContainer(TContext context)
        {
            Context = context;
        }
    }
}
