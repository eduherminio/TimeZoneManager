using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Dto
{
    public class PermissionDto : IEntityDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }
    }
}
