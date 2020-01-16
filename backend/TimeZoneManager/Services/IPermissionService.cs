using System.Collections.Generic;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Services
{
    [Log]
    [ExceptionManagement]
    public interface IPermissionService
    {
        [Authorization(PermissionName.PermissionCreate)]
        PermissionDto Create(PermissionDto dto);

        [Authorization(PermissionName.PermissionRead)]
        ICollection<PermissionDto> LoadAll();

        [Authorization(PermissionName.PermissionRead)]
        PermissionDto Load(string key);

        [Authorization(AdminPermissions.SuperAdminPermissionName)]
        void Initialize();
    }
}
