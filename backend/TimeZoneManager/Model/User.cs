using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Model
{
    public class User : Entity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<RoleInUser> RoleInUsers { get; } = new List<RoleInUser>();

        [NotMapped]
        public ICollection<Role> Roles { get; }

        [NotMapped]
        public ICollection<TimeZone> TimeZones { get; }

        public User()
        {
            Roles = new JoinCollectionFacade<Role, User, RoleInUser>(this, RoleInUsers);
        }

        public ICollection<string> GetPermissionNames()
        {
            List<string> list = new List<string>();

            var permissionThroughRoles =
                from role in Roles
                from permission in role.Permissions
                select permission.Name;

            list.AddRange(permissionThroughRoles);

            return list.Distinct().ToList();
        }
    }
}
