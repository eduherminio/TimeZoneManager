using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.Dto
{
    public class TimeZoneDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public double GmtDifferenceInHours { get; set; } = int.MinValue;

        public string Key { get; set; }

        public string OwnerUsername { get; set; }
    }
}
