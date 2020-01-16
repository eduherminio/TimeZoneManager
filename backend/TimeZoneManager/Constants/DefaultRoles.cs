using System.Collections.Generic;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;

namespace TimeZoneManager.Constants
{
    public static class DefaultRoles
    {
        public static readonly RoleDto TimeZoneManager = new RoleDto()
        {
            Name = nameof(RoleName.User),
            Description = "Regular user, capable of CRUD their own TimeZones",
            Permissions = new List<PermissionDto>()
            {
                DefaultPermissions.TimeZoneCreate, DefaultPermissions.TimeZoneRead, DefaultPermissions.TimeZoneUpdate, DefaultPermissions.TimeZoneDelete
            }
        };

        public static readonly RoleDto UserManager = new RoleDto()
        {
            Name = nameof(RoleName.UserManager),
            Description = "User manager, capable of CRUD other Users",
            Permissions = new List<PermissionDto>()
            {
                DefaultPermissions.UserCreate, DefaultPermissions.UserRead, DefaultPermissions.UserUpdate, DefaultPermissions.UserDelete,
                DefaultPermissions.RoleRead
            }
        };

        public static readonly RoleDto Admin = new RoleDto()
        {
            Name = nameof(RoleName.Admin),
            Description = "Admin user, capable of CRUD other Users and their TimeZones",
            Permissions = new List<PermissionDto>()
            {
                DefaultPermissions.UserCreate, DefaultPermissions.UserRead, DefaultPermissions.UserUpdate, DefaultPermissions.UserDelete,
                DefaultPermissions.RoleRead,
                DefaultPermissions.TimeZoneCreate, DefaultPermissions.TimeZoneRead, DefaultPermissions.TimeZoneUpdate, DefaultPermissions.TimeZoneDelete,
                DefaultPermissions.TimeZoneAdmin
            }
        };

        public static readonly IReadOnlyCollection<RoleDto> AllRoleList = new List<RoleDto>
        {
            TimeZoneManager, UserManager, Admin
        };
    }
}
