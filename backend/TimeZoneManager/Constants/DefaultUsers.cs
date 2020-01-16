using System.Collections.Generic;
using TimeZoneManager.Dto;
using TimeZoneManager.Encryption;

namespace TimeZoneManager.Constants
{
    public static class DefaultUsers
    {
        public const string TimeZone = "timezone";
        public const string User = "user";
        public const string Admn = "admin";

        public static readonly FullUserDto TimeZoneManager = new FullUserDto()
        {
            Username = TimeZone,
            Password = TimeZone.GenerateHash(),
            Roles = new List<RoleDto>()
            {
                DefaultRoles.TimeZoneManager
            }
        };

        public static readonly FullUserDto UserManager = new FullUserDto()
        {
            Username = User,
            Password = User.GenerateHash(),
            Roles = new List<RoleDto>()
            {
                DefaultRoles.UserManager
            }
        };

        public static readonly FullUserDto Admin = new FullUserDto()
        {
            Username = Admn,
            Password = Admn.GenerateHash(),
            Roles = new List<RoleDto>()
            {
                DefaultRoles.Admin
            }
        };

        public static readonly IReadOnlyCollection<FullUserDto> AllUserList = new List<FullUserDto>
        {
            TimeZoneManager,
            UserManager,
            Admin
        };
    }
}
