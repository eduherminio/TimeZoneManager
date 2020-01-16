using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TimeZoneManager.Authentication;
using TimeZoneManager.Authorization;
using TimeZoneManager.MappingProfiles;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Provider;

namespace TimeZoneManager.Test
{
    public abstract class BaseUnitTest : IDisposable
    {
        private readonly string _dbName = Guid.NewGuid().ToString();

        protected IServiceProvider ServiceProvider { get; set; }

        protected ISession Session { get; set; }

        protected BaseUnitTest()
        {
            ServiceProvider = GetBaseServiceCollection().BuildServiceProvider();
            Session = GetService<ISession>();
        }

        protected IServiceCollection GetBaseServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();

            var provider = new DbContextProvider(DbProvider.Sqlite);
            string connectionString = $"Data Source={_dbName}";
            provider.WireDbContext<DatabaseContext>(services, null, connectionString);

            services.AddJwtServices();
            ConfigureAutoMapper(services);

            services.AddGeneralServices();
            AddMockedServices(services);

            return services;
        }

        protected abstract void RenewServices();

        protected T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// New context as an admin
        /// </summary>
        protected virtual void NewContext()
        {
            NewScope();
            ((Session)Session).Permissions = new[] { AdminPermissions.AdminPermissionName };
        }

        /// <summary>
        /// New context with given permissions
        /// </summary>
        /// <param name="permission"></param>
        protected virtual void NewContext(IEnumerable<PermissionName> permissionList)
        {
            NewScope();
            ((Session)Session).Permissions = permissionList.Select(p => p.ToString());
        }

        protected virtual void AddMockedServices(IServiceCollection services) { }

        private void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(new MapperProvider().Assemblies);
        }

        private void NewScope()
        {
            IServiceScope scope = ServiceProvider.CreateScope();
            ServiceProvider = scope.ServiceProvider;
            Session = GetService<ISession>();
            GetService<DatabaseContext>().Database.EnsureCreated();
            RenewServices();
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            GetService<DatabaseContext>().Database.EnsureDeleted();
        }

        #endregion
    }
}
