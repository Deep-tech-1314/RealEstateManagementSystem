// ========================================
// FILE 2: Models/Property.cs
// ========================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateManagementSystem.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public required string Description { get; set; }

        [Required]
        [StringLength(50)]
        public required string PropertyType { get; set; } // House, Apartment, Villa, Commercial, Land

        [Required]
        [StringLength(20)]
        public required string ListingType { get; set; } // Sale, Rent

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(200)]
        public required string Address { get; set; }

        [Required]
        [StringLength(50)]
        public required string City { get; set; }

        [Required]
        [StringLength(50)]
        public required string State { get; set; }

        [Required]
        [StringLength(10)]
        public required string ZipCode { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SquareFeet { get; set; }

        public int? YearBuilt { get; set; }

        [StringLength(50)]
        public string? ParkingSpaces { get; set; }

        [Column(TypeName = "text")]
        public string? Features { get; set; } // JSON or comma-separated

        [StringLength(255)]
        public string? MainImage { get; set; } = "/images/properties/default-property.jpg";

        [Column(TypeName = "text")]
        public string? AdditionalImages { get; set; } // Comma-separated list of image paths

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Available"; // Available, Sold, Rented, Pending

        public bool IsFeatured { get; set; } = false;

        public int ViewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Inquiry> Inquiries { get; set; } = new List<Inquiry>();
    }
}
