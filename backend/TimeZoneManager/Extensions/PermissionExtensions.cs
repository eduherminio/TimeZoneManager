using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneManager.Authorization;
using TimeZoneManager.Dto;

namespace TimeZoneManager.Extensions
{
    public static class PermissionNameExtensions
    {
        public static PermissionDto ToPermissionDto(this PermissionName permission, string description)
        {
            return new PermissionDto()
            {
                Name = permission.ToString(),
                Description = description
            };
        }

        public static PermissionDto ToPermissionDto(this PermissionName permission)
        {
            return permission.ToPermissionDto(permission.ToString());
        }
    }
}
