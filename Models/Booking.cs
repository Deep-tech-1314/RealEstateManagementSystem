// ========================================
// FILE 3: Models/Booking.cs
// ========================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [StringLength(20)]
        public required string BookingTime { get; set; } // e.g., "10:00 AM"

        [StringLength(500)]
        public string? Message { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        [Column(TypeName = "text")]
        public string? AdminNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Foreign Keys
        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; } = null!;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
