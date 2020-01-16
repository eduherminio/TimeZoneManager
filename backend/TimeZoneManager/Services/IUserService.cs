using System.Collections.Generic;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;
using TimeZoneManager.Exceptions;
using TimeZoneManager.Logs;

namespace TimeZoneManager.Services
{
    [Log]
    [ExceptionManagement]
    public interface IUserService
    {
        UserDto Register(FullUserDto user);

        [Authorization(PermissionName.UserCreate)]
        UserDto Create(FullUserDto user);

        [Authorization(PermissionName.UserRead)]
        ICollection<UserDto> FindByName(string username);

        [Authorization(PermissionName.UserRead)]
        UserDto Load(string username);

        [Authorization(PermissionName.UserRead)]
        ICollection<UserDto> LoadAll();

        [Authorization(PermissionName.UserUpdate)]
        UserDto Update(UserDto user);

        [Authorization(PermissionName.UserDelete)]
        void Delete(string username);

        [Authorization(AdminPermissions.SuperAdminPermissionName)]
        void Initialize();
    }
}
