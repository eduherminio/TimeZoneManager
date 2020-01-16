using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Dto
{
    public class RoleDto : IEntityDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public ICollection<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
