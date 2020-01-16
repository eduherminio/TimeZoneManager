using Microsoft.EntityFrameworkCore;
using System;

namespace TimeZoneManager.Orm.Provider
{
    public interface IDatabaseServer
    {
        Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> UseDatabase();
    }

    public class SqlServer : IDatabaseServer
    {
        public Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> UseDatabase()
        {
            return (builder, connectionString) => builder.UseSqlServer(connectionString);
        }
    }

    public class SQLiteServer : IDatabaseServer
    {
        public Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> UseDatabase()
        {
            return (builder, connectionString) => builder.UseSqlite(connectionString);
        }
    }

    public class OracleServer : IDatabaseServer
    {
        public Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> UseDatabase()
        {
            throw new NotSupportedException();
        }
    }
}
