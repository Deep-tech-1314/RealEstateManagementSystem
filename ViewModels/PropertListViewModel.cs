using RealEstateManagementSystem.Models;
using System.Collections.Generic;

namespace RealEstateManagementSystem.ViewModels
{
    public class PropertyListViewModel
    {
        public IEnumerable<Property> Properties { get; set; } = new List<Property>();
        public PropertySearchViewModel Search { get; set; } = new PropertySearchViewModel();

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }

        public IEnumerable<string> Cities { get; set; } = new List<string>();
        public IEnumerable<string> PropertyTypes { get; set; } = new List<string>();
        public IEnumerable<string> ListingTypes { get; set; } = new List<string>();
    }
}