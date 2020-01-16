using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Authentication
{
    public class JwtTokenPayload
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public IEnumerable<string> Permissions { get; set; }

        public JwtTokenPayload()
        {
            Username = string.Empty;
            Permissions = new List<string>();
        }
    }
}
