using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Dto
{
    public class UserDto : IEntityDto
    {
        [Required]
        public string Username { get; set; }

        public string Key { get; set; }

        public ICollection<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
}
