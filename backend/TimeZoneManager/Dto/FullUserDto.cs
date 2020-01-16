using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Dto
{
    public class FullUserDto : UserDto
    {
        [Required]
        public string Password { get; set; }
    }
}
