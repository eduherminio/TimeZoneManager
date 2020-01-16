using System;
using System.ComponentModel.DataAnnotations.Schema;
using TimeZoneManager.Model;
using TimeZoneManager.Orm.Model;

namespace TimeZoneManager.Model
{
    public class RoleInUser : IJoinEntity<User>, IJoinEntity<Role>
    {
        [ForeignKey("UserId"), Column(Order = 0)]
        public Guid UserId { get; set; }

        public User User { get; set; }

        User IJoinEntity<User>.Navigation
        {
            get => User;
            set => User = value;
        }

        [ForeignKey("RoleId"), Column(Order = 1)]
        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        Role IJoinEntity<Role>.Navigation
        {
            get => Role;
            set => Role = value;
        }
    }
}
