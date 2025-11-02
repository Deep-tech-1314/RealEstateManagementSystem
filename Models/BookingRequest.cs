using System.ComponentModel.DataAnnotations;

namespace RealEstateManagementSystem.Models
{
    public class BookingRequest
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string BookingTime { get; set; } = string.Empty;

        public string? Message { get; set; }
    }
}
