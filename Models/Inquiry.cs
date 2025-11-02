// ========================================
// FILE 4: Models/Inquiry.cs
// ========================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [Column(TypeName = "text")]
        public required string Message { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "New"; // New, Read, Replied

        [Column(TypeName = "text")]
        public string? AdminReply { get; set; }

        public DateTime? RepliedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign Keys (Optional - can be null for general inquiries)
        [ForeignKey("Property")]
        public int? PropertyId { get; set; }
        public virtual Property? Property { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}