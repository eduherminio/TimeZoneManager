using System;
using System.ComponentModel.DataAnnotations.Schema;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Model
{
    public class PermissionInRole : IJoinEntity<Role>, IJoinEntity<Permission>
    {
        [ForeignKey("RoleId"), Column(Order = 0)]
        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        Role IJoinEntity<Role>.Navigation
        {
            get => Role;
            set => Role = value;
        }

        [ForeignKey("PermissionId"), Column(Order = 1)]
        public Guid PermissionId { get; set; }

        public Permission Permission { get; set; }

        Permission IJoinEntity<Permission>.Navigation
        {
            get => Permission;
            set => Permission = value;
        }
    }
}
