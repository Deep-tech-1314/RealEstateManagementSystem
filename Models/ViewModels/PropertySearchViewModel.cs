namespace RealEstateManagementSystem.ViewModels
{
    public class PropertySearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? PropertyType { get; set; }
        public string? ListingType { get; set; }
        public string? City { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }

        // Sorting and pagination
        public string? SortBy { get; set; } // price_asc, price_desc, newest, oldest, popular
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}