using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TimeZoneManager.Orm.Provider
{
    public interface IDbContextProvider
    {
        void WireDbContext<TDbContext>(IServiceCollection services, IConfiguration configuration, string connectionString)
            where TDbContext : DbContext;
    }

    public class DbContextProvider : IDbContextProvider
    {
        private readonly IDatabaseOptions _databaseOptions;

        public DbContextProvider(DbProvider provider) : this(new DatabaseOptions(provider))
        {
        }

        public DbContextProvider(IDatabaseOptions databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void WireDbContext<TDbContext>(IServiceCollection services, IConfiguration configuration, string connectionString)
            where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(_databaseOptions.GetDatabaseOptions(connectionString), ServiceLifetime.Scoped);
            services.AddScoped<IDatabaseContextContainer, DatabaseContextContainer<DatabaseContext>>();
        }
    }
}
