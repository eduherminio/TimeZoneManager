using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace TimeZoneManager.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AuthorizationAttribute : AbstractInterceptorAttribute
    {
        private const string UserJustLoggedIn = "";

        private readonly ICollection<string> _requiredPermissions;

        public AuthorizationAttribute()
        {
            _requiredPermissions = new HashSet<string>() { UserJustLoggedIn };
        }

        public AuthorizationAttribute(string permission)
             : this(new[] { permission })
        {
        }

        public AuthorizationAttribute(params string[] permissions)
        {
            _requiredPermissions = permissions.ToHashSet();
        }

        public AuthorizationAttribute(PermissionName permission)
            : this(permission.ToString())
        {
        }

        public AuthorizationAttribute(params PermissionName[] permission)
            : this(permission.Select(p => p.ToString()).ToArray())
        {
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            ISession session = context.ServiceProvider.GetRequiredService<ISession>();
            if (_requiredPermissions.Contains(UserJustLoggedIn) && session.IsAuthenticated())
            {
                await next(context).ConfigureAwait(false);
            }
            else
            {
                if (HasRequiredPermissions(session.Permissions))
                {
                    await next(context).ConfigureAwait(false);
                }
                else
                {
                    throw new UnauthorizedAccessException("Authorization required");
                }
            }
        }

        private bool HasRequiredPermissions(IEnumerable<string> permissions)
        {
            return permissions.Contains(AdminPermissions.AdminPermissionName)
                || permissions.Contains(AdminPermissions.SuperAdminPermissionName)
                || permissions.Intersect(_requiredPermissions).Count() == _requiredPermissions.Count;
        }
    }
}
