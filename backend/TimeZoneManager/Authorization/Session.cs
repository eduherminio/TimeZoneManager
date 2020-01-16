using System.Collections.Generic;
using System.Linq;

namespace TimeZoneManager.Authorization
{
    public class Session : ISession
    {
        public string Username { get; set; }

        public IEnumerable<string> Permissions { get; set; }

        public string Token { get; set; }

        public Session()
        {
            Username = string.Empty;
            Permissions = new List<string>();
            Token = string.Empty;
        }

        public bool IsAuthenticated()
        {
            return Permissions.Any();
        }

        public bool IsAdmin()
        {
            return Permissions.Contains(AdminPermissions.SuperAdminPermissionName)
                || Permissions.Contains(AdminPermissions.AdminPermissionName);
        }
    }
}
