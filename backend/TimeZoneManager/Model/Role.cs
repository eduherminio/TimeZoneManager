using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Model
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PermissionInRole> PermissionInRoles { get; } = new List<PermissionInRole>();

        public ICollection<RoleInUser> RoleInUsers { get; } = new List<RoleInUser>();

        [NotMapped]
        public ICollection<Permission> Permissions { get; }

        [NotMapped]
        public IEnumerable<User> Users { get; }

        public Role()
        {
            Permissions = new JoinCollectionFacade<Permission, Role, PermissionInRole>(this, PermissionInRoles);
            Users = new JoinCollectionFacade<User, Role, RoleInUser>(this, RoleInUsers);
        }
    }
}
