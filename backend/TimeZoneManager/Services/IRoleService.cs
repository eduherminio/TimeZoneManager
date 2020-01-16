using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Services
{
    [Log]
    [ExceptionManagement]
    public interface IRoleService
    {
        [Authorization(PermissionName.RoleCreate)]
        RoleDto Create(RoleDto dto);

        [Authorization(PermissionName.RoleUpdate)]
        RoleDto Update(RoleDto dto);

        [Authorization(PermissionName.RoleRead)]
        ICollection<RoleDto> LoadAll();

        [Authorization(PermissionName.RoleRead)]
        RoleDto Load(string key);

        [Authorization(PermissionName.RoleDelete)]
        void Delete(string key);

        [Authorization(AdminPermissions.SuperAdminPermissionName)]
        void Initialize();
    }
}
