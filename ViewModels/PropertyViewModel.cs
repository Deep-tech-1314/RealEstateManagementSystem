using Microsoft.AspNetCore.Http;

namespace RealEstateManagementSystem.ViewModels
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; } // <-- Add this property
        public int Id { get; set; } // <-- Add this property for compatibility

        public required string Title { get; set; }
        public required string PropertyType { get; set; }
        public required string ListingType { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? SquareFeet { get; set; }
        public int? YearBuilt { get; set; }
        public string? ParkingSpaces { get; set; }
        public string? Features { get; set; }
        public IFormFile? MainImage { get; set; }
        public IEnumerable<IFormFile>? AdditionalImages { get; set; }
        public required string Status { get; set; }
        public bool IsFeatured { get; set; }
        public bool KeepExistingImages { get; set; } = true;
    }
}