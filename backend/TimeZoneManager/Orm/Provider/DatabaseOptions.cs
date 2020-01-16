using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace TimeZoneManager.Orm.Provider
{
    public interface IDatabaseOptions
    {
        Action<IServiceProvider, DbContextOptionsBuilder> GetDatabaseOptions(params string[] connectionStrings);
    }

    public class DatabaseOptions : IDatabaseOptions
    {
        private static readonly IReadOnlyDictionary<DbProvider, IDatabaseServer> _defaultDatabaseServer =
           new Dictionary<DbProvider, IDatabaseServer> {
                {DbProvider.SqlServer, new SqlServer() },
                {DbProvider.Sqlite, new SQLiteServer() },
                {DbProvider.Oracle, new OracleServer() }
           };

        protected IDatabaseServer DatabaseServer { get; }

        public DatabaseOptions(DbProvider provider) : this(_defaultDatabaseServer[provider])
        {
        }

        protected DatabaseOptions(IDatabaseServer databaseServer)
        {
            DatabaseServer = databaseServer;
        }

        public virtual Action<IServiceProvider, DbContextOptionsBuilder> GetDatabaseOptions(params string[] connectionStrings)
        {
            return (_, builder) =>
            {
                DatabaseServer.UseDatabase()(builder, connectionStrings[0]);
                builder.EnableSensitiveDataLogging();
            };
        }
    }
}
