using System.ComponentModel.DataAnnotations;

namespace RealEstateManagementSystem.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        [StringLength(100)]
        public required string CityName { get; set; }

        [Required]
        [StringLength(100)]
        public required string State { get; set; }

        [StringLength(500)]
        public string? CityImage { get; set; }

        public int PropertyCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}