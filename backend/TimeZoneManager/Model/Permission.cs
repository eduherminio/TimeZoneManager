using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TimeZoneManager.Orm;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Model
{
    public class Permission : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PermissionInRole> PermissionInRoles { get; } = new List<PermissionInRole>();

        [NotMapped]
        public IEnumerable<Role> Roles { get; }

        public Permission()
        {
            Roles = new JoinCollectionFacade<Role, Permission, PermissionInRole>(this, PermissionInRoles);
        }
    }
}
