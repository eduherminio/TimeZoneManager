using System.Collections.Generic;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Extensions;

namespace TimeZoneManager.Constants
{
    internal static class DefaultPermissions
    {
        //public static readonly PermissionDto Admin = new PermissionDto { Name = AdminPermissions.AdminPermissionName };
        internal static readonly PermissionDto SuperAdmin = new PermissionDto { Name = AdminPermissions.SuperAdminPermissionName };

        public static readonly PermissionDto PermissionCreate = PermissionName.PermissionCreate.ToPermissionDto();
        public static readonly PermissionDto PermissionRead = PermissionName.PermissionRead.ToPermissionDto();

        public static readonly PermissionDto RoleCreate = PermissionName.RoleCreate.ToPermissionDto();
        public static readonly PermissionDto RoleRead = PermissionName.RoleRead.ToPermissionDto();
        public static readonly PermissionDto RoleUpdate = PermissionName.RoleUpdate.ToPermissionDto();
        public static readonly PermissionDto RoleDelete = PermissionName.RoleDelete.ToPermissionDto();

        public static readonly PermissionDto UserCreate = PermissionName.UserCreate.ToPermissionDto();
        public static readonly PermissionDto UserRead = PermissionName.UserRead.ToPermissionDto();
        public static readonly PermissionDto UserUpdate = PermissionName.UserUpdate.ToPermissionDto();
        public static readonly PermissionDto UserDelete = PermissionName.UserDelete.ToPermissionDto();

        public static readonly PermissionDto TimeZoneCreate = PermissionName.TimeZoneCreate.ToPermissionDto();
        public static readonly PermissionDto TimeZoneRead = PermissionName.TimeZoneRead.ToPermissionDto();
        public static readonly PermissionDto TimeZoneUpdate = PermissionName.TimeZoneUpdate.ToPermissionDto();
        public static readonly PermissionDto TimeZoneDelete = PermissionName.TimeZoneDelete.ToPermissionDto();
        public static readonly PermissionDto TimeZoneAdmin = PermissionName.TimeZoneAdmin.ToPermissionDto();

        public static readonly IReadOnlyCollection<PermissionDto> AllPermissionsList = new List<PermissionDto>
        {
            //SuperAdmin,

            //Admin,

            PermissionCreate, PermissionRead,

            RoleCreate, RoleRead, RoleUpdate, RoleDelete,

            UserCreate, UserRead, UserUpdate, UserDelete,

            TimeZoneCreate, TimeZoneRead, TimeZoneUpdate, TimeZoneDelete, TimeZoneAdmin
        };
    }
}
