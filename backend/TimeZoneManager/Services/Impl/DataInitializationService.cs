using Microsoft.Extensions.DependencyInjection;
using System;
using TimeZoneManager.Authorization;

namespace TimeZoneManager.Services.Impl
{
    public class DataInitializationService : IDataInitializationService
    {
        private readonly IServiceProvider _serviceProvider;

        public DataInitializationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                SetAuthorizedSession(scope);
                scope.ServiceProvider.GetRequiredService<IPermissionService>().Initialize();
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                SetAuthorizedSession(scope);
                scope.ServiceProvider.GetRequiredService<IRoleService>().Initialize();
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                SetAuthorizedSession(scope);
                scope.ServiceProvider.GetRequiredService<IUserService>().Initialize();
            }
        }

        private static void SetAuthorizedSession(IServiceScope scope)
        {
            ISession session = scope.ServiceProvider.GetRequiredService<ISession>();
            ((Session)session).Permissions = new[] { AdminPermissions.SuperAdminPermissionName };
        }
    }
}
