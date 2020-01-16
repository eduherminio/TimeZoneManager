using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeZoneManager.Orm;

namespace TimeZoneManager.Model
{
    public class TimeZone : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public double GmtDifferenceInHours { get; set; }

        /// <summary>
        /// User's username of entity's owner
        /// </summary>
        [Required]
        public System.Guid UserId { get; set; }

        [NotMapped]
        public User User { get; set; }
    }
}
