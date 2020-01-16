using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TimeZoneManager.Constants;
using TimeZoneManager.Dao;
using TimeZoneManager.Dao.Impl;
using TimeZoneManager.MappingProfiles;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Provider;
using TimeZoneManager.Services;
using TimeZoneManager.Services.Impl;

namespace TimeZoneManager
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTimeZoneManagerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGeneralServices();
            services.AddDatabasWiring(configuration);
        }

        internal static void AddGeneralServices(this IServiceCollection services)
        {
            services.AddServices();
            services.AddDaos();
        }

        private static void AddDatabasWiring(this IServiceCollection services, IConfiguration configuration)
        {
            services.WireDbContext(configuration);
            services.ConfigureAutoMapper();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IDataInitializationService, DataInitializationService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ITimeZoneService, TimeZoneService>();
        }

        private static void AddDaos(this IServiceCollection services)
        {
            services.AddScoped<IUserDao, UserDaoEfImpl>();
            services.AddScoped<IRoleDao, RoleDaoEfImpl>();
            services.AddScoped<IPermissionDao, PermissionDaoEfImpl>();
            services.AddScoped<ITimeZoneDao, TimeZoneDaoEfImpl>();
        }

        private static void WireDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = new DbContextProvider(DbProvider.SqlServer);
            provider.WireDbContext<DatabaseContext>(services, configuration, configuration[AppSettingsKeys.DatabaseConnectionString]);
        }

        private static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(new MapperProvider().Assemblies);
        }
    }
}
