
// Data/DatabaseSeeder.cs

using RealEstateManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace RealEstateManagementSystem.Data
{
    public static class DatabaseSeeder
{
    // Helper method to check if an image was uploaded by an admin
    private static bool IsAdminUploadedImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return false;
            
        // Admin-uploaded images have a GUID in their filename
        // Example: /images/properties/12345678-1234-1234-1234-123456789012_image.jpg
        return imagePath.Contains("/images/properties/") && 
               imagePath.Contains("_") && 
               !imagePath.Contains("default-property.jpg") &&
               !imagePath.Contains("unsplash.com");
    }
    
    public static void SeedData(ApplicationDbContext context)
{
    // Seed Cities First
    if (!context.Cities.Any())
            {
                var cities = new List<City>
                {
                    new City
                    {
                        CityName = "Delhi / NCR",
                        State = "Delhi",
                        CityImage = "https://images.unsplash.com/photo-1587474260584-136574528ed5?w=400&h=300&fit=crop",
                        PropertyCount = 248000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Bangalore",
                        State = "Karnataka",
                        CityImage = "https://images.unsplash.com/photo-1596176530529-78163a4f7af2?w=400&h=300&fit=crop",
                        PropertyCount = 69000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Pune",
                        State = "Maharashtra",
                        CityImage = "https://images.unsplash.com/photo-1595659443965-7ab2f7f50cdb?w=400&h=300&fit=crop",
                        PropertyCount = 63000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Chennai",
                        State = "Tamil Nadu",
                        CityImage = "https://images.unsplash.com/photo-1582510003544-4d00b7f74220?w=400&h=300&fit=crop",
                        PropertyCount = 45000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Mumbai",
                        State = "Maharashtra",
                        CityImage = "https://images.unsplash.com/photo-1529253355930-ddbe423a2ac7?w=400&h=300&fit=crop",
                        PropertyCount = 69000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Hyderabad",
                        State = "Telangana",
                        CityImage = "https://images.unsplash.com/photo-1609920658906-8223bd289001?w=400&h=300&fit=crop",
                        PropertyCount = 35000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Kolkata",
                        State = "West Bengal",
                        CityImage = "https://images.unsplash.com/photo-1558431382-27e303142255?w=400&h=300&fit=crop",
                        PropertyCount = 39000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new City
                    {
                        CityName = "Ahmedabad",
                        State = "Gujarat",
                        CityImage = "https://images.unsplash.com/photo-1631035831324-90464e7fc50f?w=400&h=300&fit=crop",
                        PropertyCount = 32000,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                };

                context.Cities.AddRange(cities);
                context.SaveChanges();
            }

            // Check if properties already seeded
            if (context.Properties.Any())
            {
                // Restore original images for existing properties
                RestoreOriginalImages(context);
                return; // Database already seeded
            }

            // Get admin user (assuming UserId = 1)
            var adminId = 1;

            // Check for existing properties to preserve admin-uploaded images
            var existingProperties = context.Properties.ToList();
            
            // Sample Properties Data - INDIAN LOCATIONS
            var properties = new List<Property>
            {
                // HOUSES (5 properties) - INDIAN LOCATIONS
                new Property
                {
                    Title = "Luxury 4BHK Villa in DLF Phase 5",
                    Description = "Spacious 4-bedroom independent house with beautiful garden, modular kitchen, and private parking.",
                    PropertyType = "House",
                    ListingType = "Sale",
                    Price = 45000000, // 4.5 Crore INR
                    Address = "DLF Phase 5, Sector 53",
                    City = "Gurgaon",
                    State = "Haryana",
                    ZipCode = "122009",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    SquareFeet = 2500,
                    YearBuilt = 2018,
                    ParkingSpaces = "2-car garage",
                    Features = "Swimming Pool, Garden, Modular Kitchen, Servant Quarter, Power Backup",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=500&h=350&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Cozy 3BHK House in Whitefield",
                    Description = "Perfect family home in IT hub with great schools and hospitals nearby.",
                    PropertyType = "House",
                    ListingType = "Sale",
                    Price = 12500000, // 1.25 Crore INR
                    Address = "Whitefield Main Road",
                    City = "Bangalore",
                    State = "Karnataka",
                    ZipCode = "560066",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    SquareFeet = 1800,
                    YearBuilt = 2015,
                    ParkingSpaces = "Covered parking",
                    Features = "Garden, Modular Kitchen, Vitrified Flooring, 24x7 Security",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "/images/properties/h3.jpg",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Modern Row House in Baner",
                    Description = "Contemporary 3-bedroom row house with terrace garden in prime location.",
                    PropertyType = "House",
                    ListingType = "Rent",
                    Price = 65000, // Monthly rent
                    Address = "Baner Road, Near Balewadi Stadium",
                    City = "Pune",
                    State = "Maharashtra",
                    ZipCode = "411045",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    SquareFeet = 1600,
                    YearBuilt = 2020,
                    ParkingSpaces = "1-car garage",
                    Features = "Terrace Garden, Smart Home, Open Kitchen, Club House Access",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "/images/properties/h4.jpg",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Traditional House in Anna Nagar",
                    Description = "Well-maintained 2-bedroom independent house with car parking.",
                    PropertyType = "House",
                    ListingType = "Sale",
                    Price = 8500000, // 85 Lakhs INR
                    Address = "Anna Nagar West Extension",
                    City = "Chennai",
                    State = "Tamil Nadu",
                    ZipCode = "600101",
                    Bedrooms = 2,
                    Bathrooms = 1,
                    SquareFeet = 1200,
                    YearBuilt = 1985,
                    ParkingSpaces = "Open parking",
                    Features = "Bore Well, Garden, Traditional Architecture, Corner Plot",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "/images/properties/h4.jpg",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Luxury Bungalow in Juhu",
                    Description = "Elegant 5-bedroom sea-facing bungalow with private pool.",
                    PropertyType = "House",
                    ListingType = "Sale",
                    Price = 250000000, // 25 Crore INR
                    Address = "Juhu Tara Road",
                    City = "Mumbai",
                    State = "Maharashtra",
                    ZipCode = "400049",
                    Bedrooms = 5,
                    Bathrooms = 4,
                    SquareFeet = 4000,
                    YearBuilt = 2019,
                    ParkingSpaces = "3-car garage",
                    Features = "Sea View, Private Pool, Home Theater, Gym, Italian Marble",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=500&h=350&fit=crop",
                    CreatedAt = DateTime.Now
                },

                // APARTMENTS (5 properties)
                new Property
                {
                    Title = "Premium 2BHK in Lodha Altamount",
                    Description = "Modern 2-bedroom apartment in South Mumbai's most prestigious address.",
                    PropertyType = "Apartment",
                    ListingType = "Rent",
                    Price = 250000, // Monthly rent
                    Address = "Altamount Road, Kemps Corner",
                    City = "Mumbai",
                    State = "Maharashtra",
                    ZipCode = "400026",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    SquareFeet = 1100,
                    YearBuilt = 2021,
                    ParkingSpaces = "2 reserved spots",
                    Features = "Gym, Swimming Pool, Concierge, Sky Lounge, High-Speed Elevators",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "/images/properties/h5.jpg",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Studio Apartment in Cyber City",
                    Description = "Fully furnished studio apartment perfect for young professionals.",
                    PropertyType = "Apartment",
                    ListingType = "Rent",
                    Price = 35000, // Monthly rent
                    Address = "DLF Cyber City, Phase 2",
                    City = "Gurgaon",
                    State = "Haryana",
                    ZipCode = "122002",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    SquareFeet = 600,
                    YearBuilt = 2020,
                    ParkingSpaces = "Common parking",
                    Features = "Furnished, Gym, Power Backup, Security, Metro Connectivity",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "/images/properties/a2.jpg",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Penthouse in Koregaon Park",
                    Description = "Luxurious penthouse with panoramic city views and private terrace.",
                    PropertyType = "Apartment",
                    ListingType = "Sale",
                    Price = 55000000, // 5.5 Crore INR
                    Address = "Koregaon Park Plaza",
                    City = "Pune",
                    State = "Maharashtra",
                    ZipCode = "411001",
                    Bedrooms = 3,
                    Bathrooms = 3,
                    SquareFeet = 2200,
                    YearBuilt = 2022,
                    ParkingSpaces = "2 reserved spots",
                    Features = "City View, Private Terrace, Italian Kitchen, Smart Home, Private Lift",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1500382017468-9049fed747ef?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Lake View Apartment in Powai",
                    Description = "Spacious 1-bedroom apartment with beautiful lake views.",
                    PropertyType = "Apartment",
                    ListingType = "Rent",
                    Price = 45000, // Monthly rent
                    Address = "Hiranandani Gardens, Powai",
                    City = "Mumbai",
                    State = "Maharashtra",
                    ZipCode = "400076",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    SquareFeet = 850,
                    YearBuilt = 2018,
                    ParkingSpaces = "1 spot",
                    Features = "Lake View, Club House, Swimming Pool, Jogging Track",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1599127585910-28f499007ed8?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=2076",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Modern 2BHK in Electronic City",
                    Description = "Contemporary apartment in Bangalore's IT corridor.",
                    PropertyType = "Apartment",
                    ListingType = "Rent",
                    Price = 32000, // Monthly rent
                    Address = "Electronic City Phase 1",
                    City = "Bangalore",
                    State = "Karnataka",
                    ZipCode = "560100",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    SquareFeet = 1400,
                    YearBuilt = 2017,
                    ParkingSpaces = "1 covered spot",
                    Features = "Gym, Children's Play Area, Power Backup, Intercom",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1615621378629-00821b494d9e?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    CreatedAt = DateTime.Now
                },

                // VILLAS (5 properties)
                new Property
                {
                    Title = "Beach Villa in ECR Chennai",
                    Description = "Stunning beachfront villa with private beach access.",
                    PropertyType = "Villa",
                    ListingType = "Sale",
                    Price = 120000000, // 12 Crore INR
                    Address = "East Coast Road, Neelankarai",
                    City = "Chennai",
                    State = "Tamil Nadu",
                    ZipCode = "600115",
                    Bedrooms = 6,
                    Bathrooms = 5,
                    SquareFeet = 5500,
                    YearBuilt = 2020,
                    ParkingSpaces = "4-car garage",
                    Features = "Beach Access, Private Pool, Landscaped Garden, Home Theater, Gym",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Hill View Villa in Lonavala",
                    Description = "Secluded villa with breathtaking valley views.",
                    PropertyType = "Villa",
                    ListingType = "Rent",
                    Price = 150000, // Monthly rent
                    Address = "Tungarli Lake Road",
                    City = "Lonavala",
                    State = "Maharashtra",
                    ZipCode = "410401",
                    Bedrooms = 5,
                    Bathrooms = 4,
                    SquareFeet = 4500,
                    YearBuilt = 2019,
                    ParkingSpaces = "3-car garage",
                    Features = "Valley View, Private Pool, BBQ Area, Fireplace, Caretaker Room",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Spanish Villa in Vagator",
                    Description = "Mediterranean-style villa near Vagator beach.",
                    PropertyType = "Villa",
                    ListingType = "Sale",
                    Price = 85000000, // 8.5 Crore INR
                    Address = "Vagator Beach Road",
                    City = "Goa",
                    State = "Goa",
                    ZipCode = "403509",
                    Bedrooms = 4,
                    Bathrooms = 4,
                    SquareFeet = 3800,
                    YearBuilt = 2018,
                    ParkingSpaces = "3-car garage",
                    Features = "Pool, Courtyard, Outdoor Kitchen, Guest House, Beach Access",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1613977257592-4871e5fcd7c4?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Smart Villa in Kokapet",
                    Description = "Ultra-modern villa with smart home technology.",
                    PropertyType = "Villa",
                    ListingType = "Sale",
                    Price = 65000000, // 6.5 Crore INR
                    Address = "Financial District, Kokapet",
                    City = "Hyderabad",
                    State = "Telangana",
                    ZipCode = "500075",
                    Bedrooms = 5,
                    Bathrooms = 5,
                    SquareFeet = 4800,
                    YearBuilt = 2022,
                    ParkingSpaces = "4-car garage",
                    Features = "Smart Home, Pool, Home Office, Solar Panels, EV Charging",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Lakeside Villa in Udaipur",
                    Description = "Heritage-style villa with lake views.",
                    PropertyType = "Villa",
                    ListingType = "Rent",
                    Price = 200000, // Monthly rent
                    Address = "Fateh Sagar Lake Road",
                    City = "Udaipur",
                    State = "Rajasthan",
                    ZipCode = "313001",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    SquareFeet = 3500,
                    YearBuilt = 2017,
                    ParkingSpaces = "2-car garage",
                    Features = "Lake View, Traditional Architecture, Pool, Garden, Boat Access",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500&h=350&fit=crop",
                    CreatedAt = DateTime.Now
                },

                // COMMERCIAL (5 properties)
                new Property
                {
                    Title = "Prime Office Space in BKC",
                    Description = "Premium office space in Bandra Kurla Complex.",
                    PropertyType = "Commercial",
                    ListingType = "Rent",
                    Price = 500000, // Monthly rent
                    Address = "Bandra Kurla Complex",
                    City = "Mumbai",
                    State = "Maharashtra",
                    ZipCode = "400051",
                    Bedrooms = null,
                    Bathrooms = 3,
                    SquareFeet = 5000,
                    YearBuilt = 2021,
                    ParkingSpaces = "20 spots",
                    Features = "Conference Rooms, Cafeteria, Reception, High-Speed Internet, 24x7 Access",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1497366754035-f200968a6e72?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Retail Space in Connaught Place",
                    Description = "High-footfall retail space in Delhi's prime shopping district.",
                    PropertyType = "Commercial",
                    ListingType = "Rent",
                    Price = 350000, // Monthly rent
                    Address = "Connaught Place, Block A",
                    City = "New Delhi",
                    State = "Delhi",
                    ZipCode = "110001",
                    Bedrooms = null,
                    Bathrooms = 2,
                    SquareFeet = 2000,
                    YearBuilt = 2015,
                    ParkingSpaces = "Public parking",
                    Features = "Prime Location, Large Frontage, Storage, High Footfall",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1606836576983-8b458e75221d?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Warehouse in Bhiwandi",
                    Description = "Large warehouse with excellent connectivity to Mumbai and Thane.",
                    PropertyType = "Commercial",
                    ListingType = "Sale",
                    Price = 75000000, // 7.5 Crore INR
                    Address = "Mumbai-Nashik Highway, Bhiwandi",
                    City = "Bhiwandi",
                    State = "Maharashtra",
                    ZipCode = "421302",
                    Bedrooms = null,
                    Bathrooms = 4,
                    SquareFeet = 25000,
                    YearBuilt = 2016,
                    ParkingSpaces = "50 spots",
                    Features = "Loading Docks, Office Space, High Ceiling, Fire Safety System",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://plus.unsplash.com/premium_photo-1750294335185-a733135e5524?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1332",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "IT Office Space in HITEC City",
                    Description = "Modern office space in Hyderabad's IT hub.",
                    PropertyType = "Commercial",
                    ListingType = "Rent",
                    Price = 250000, // Monthly rent
                    Address = "HITEC City, Madhapur",
                    City = "Hyderabad",
                    State = "Telangana",
                    ZipCode = "500081",
                    Bedrooms = null,
                    Bathrooms = 6,
                    SquareFeet = 4000,
                    YearBuilt = 2019,
                    ParkingSpaces = "30 spots",
                    Features = "Plug-and-Play, Cafeteria, Conference Rooms, Metro Connectivity",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1577760258779-e787a1733016?w=800&auto=format&fit=crop",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Restaurant Space in Park Street",
                    Description = "Fully equipped restaurant space in Kolkata's food hub.",
                    PropertyType = "Commercial",
                    ListingType = "Rent",
                    Price = 300000, // Monthly rent
                    Address = "Park Street",
                    City = "Kolkata",
                    State = "West Bengal",
                    ZipCode = "700016",
                    Bedrooms = null,
                    Bathrooms = 3,
                    SquareFeet = 3500,
                    YearBuilt = 2020,
                    ParkingSpaces = "Valet parking",
                    Features = "Commercial Kitchen, Bar License, Outdoor Seating, Storage",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://plus.unsplash.com/premium_photo-1670984940113-f3aa1cd1309a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    CreatedAt = DateTime.Now
                },

                // LAND (5 properties)
                new Property
                {
                    Title = "Residential Plot in Sector 150 Noida",
                    Description = "Prime residential plot in developing sector.",
                    PropertyType = "Land",
                    ListingType = "Sale",
                    Price = 35000000, // 3.5 Crore INR
                    Address = "Sector 150, Sports City",
                    City = "Noida",
                    State = "Uttar Pradesh",
                    ZipCode = "201310",
                    Bedrooms = null,
                    Bathrooms = null,
                    SquareFeet = 4356, // 400 sq yards
                    YearBuilt = null,
                    ParkingSpaces = null,
                    Features = "Corner Plot, Park Facing, Wide Road, Gated Community",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://plus.unsplash.com/premium_photo-1733259750860-58ea1a58df12?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1332",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Agricultural Land in Nashik",
                    Description = "Fertile farmland suitable for grape cultivation.",
                    PropertyType = "Land",
                    ListingType = "Sale",
                    Price = 15000000, // 1.5 Crore INR
                    Address = "Nashik-Pune Highway",
                    City = "Nashik",
                    State = "Maharashtra",
                    ZipCode = "422001",
                    Bedrooms = null,
                    Bathrooms = null,
                    SquareFeet = 217800, // 5 acres
                    YearBuilt = null,
                    ParkingSpaces = null,
                    Features = "Water Source, Electricity, Road Access, Fertile Soil",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://plus.unsplash.com/premium_photo-1726313836345-3524772937fe?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Beach Front Land in Alibaug",
                    Description = "Prime beachfront plot for villa development.",
                    PropertyType = "Land",
                    ListingType = "Sale",
                    Price = 65000000, // 6.5 Crore INR
                    Address = "Alibaug Beach Road",
                    City = "Alibaug",
                    State = "Maharashtra",
                    ZipCode = "402201",
                    Bedrooms = null,
                    Bathrooms = null,
                    SquareFeet = 10890, // 1000 sq yards
                    YearBuilt = null,
                    ParkingSpaces = null,
                    Features = "Beach Access, Clear Title, Road Access, Electricity Available",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://plus.unsplash.com/premium_photo-1664283661436-7b3f6e3416e8?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1173",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Commercial Land on NH-48",
                    Description = "Prime commercial land on Delhi-Jaipur highway.",
                    PropertyType = "Land",
                    ListingType = "Sale",
                    Price = 45000000, // 4.5 Crore INR
                    Address = "NH-48, Near Manesar",
                    City = "Gurgaon",
                    State = "Haryana",
                    ZipCode = "122050",
                    Bedrooms = null,
                    Bathrooms = null,
                    SquareFeet = 43560, // 1 acre
                    YearBuilt = null,
                    ParkingSpaces = null,
                    Features = "Highway Frontage, Commercial Zone, High Visibility",
                    Status = "Available",
                    IsFeatured = false,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1638969992043-deaf7ff8a445?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1170",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Title = "Hill Station Plot in Mussoorie",
                    Description = "Scenic plot with valley views in Queen of Hills.",
                    PropertyType = "Land",
                    ListingType = "Sale",
                    Price = 18000000, // 1.8 Crore INR
                    Address = "Landour Cantonment",
                    City = "Mussoorie",
                    State = "Uttarakhand",
                    ZipCode = "248179",
                    Bedrooms = null,
                    Bathrooms = null,
                    SquareFeet = 5445, // 500 sq yards
                    YearBuilt = null,
                    ParkingSpaces = null,
                    Features = "Valley View, Pine Trees, Road Access, Building Permission",
                    Status = "Available",
                    IsFeatured = true,
                    OwnerId = adminId,
                    MainImage = "https://images.unsplash.com/photo-1564555952900-98b3d8bebcd2?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8N3x8SGlsbCUyMFN0YXRpb24lMjBQbG90JTIwaW4lMjBNdXNzb29yaWV8ZW58MHx8MHx8fDA%3D&auto=format&fit=crop&q=60&w=600",
                    CreatedAt = DateTime.Now
                }
            };

            // Add all properties to database
            // Before adding properties, check if they already exist and preserve admin-uploaded images
            foreach (var property in properties)
            {
                // Check if this property already exists in the database
                var existingProperty = existingProperties.FirstOrDefault(p => 
                    p.Title == property.Title && 
                    p.Address == property.Address && 
                    p.City == property.City);
                
                // If property exists and has an admin-uploaded image, preserve it
                if (existingProperty != null && !string.IsNullOrEmpty(existingProperty.MainImage) && IsAdminUploadedImage(existingProperty.MainImage))
                {
                    // Keep the admin-uploaded image
                    property.MainImage = existingProperty.MainImage;
                    
                    // Also preserve additional images if they exist
                    if (!string.IsNullOrEmpty(existingProperty.AdditionalImages))
                    {
                        property.AdditionalImages = existingProperty.AdditionalImages;
                    }
                }
            }
            
            // Remove existing properties before adding new ones
            context.Properties.RemoveRange(existingProperties);
            
            // Add the new properties (with preserved images where applicable)
            context.Properties.AddRange(properties);
            context.SaveChanges();
        }

        public static void RestoreOriginalImages(ApplicationDbContext context)
        {
            // Define the original images from DatabaseSeeder
            var originalImages = new Dictionary<string, string>
            {
                // Houses
                ["Luxury 4BHK Villa in DLF Phase 5"] = "https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=500&h=350&fit=crop",
                ["Modern 3BHK House in Whitefield"] = "https://images.unsplash.com/photo-1576941089067-2de3c901e126?w=500&h=350&fit=crop",
                ["Spacious 5BHK House in Banjara Hills"] = "https://images.unsplash.com/photo-1598228723793-52759bba239c?w=500&h=350&fit=crop",
                ["Beautiful 3BHK House in Koregaon Park"] = "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=500&h=350&fit=crop",
                ["Elegant 4BHK House in Salt Lake"] = "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=500&h=350&fit=crop",
                
                // Apartments
                ["Premium 2BHK Apartment in Hiranandani"] = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500&h=350&fit=crop",
                ["Luxury 3BHK Apartment in Gachibowli"] = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=500&h=350&fit=crop",
                ["Modern 2BHK Apartment in Hinjewadi"] = "https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=500&h=350&fit=crop",
                ["Spacious 3BHK Apartment in T. Nagar"] = "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500&h=350&fit=crop",
                ["High-end 2BHK Apartment in Powai"] = "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=500&h=350&fit=crop",
                
                // Villas
                ["Luxury Villa in Palm Beach Road"] = "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=500&h=350&fit=crop",
                ["Premium Villa in Jubilee Hills"] = "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=500&h=350&fit=crop",
                ["Exclusive Villa in Lavasa"] = "https://images.unsplash.com/photo-1613977257592-4871e5fcd7c4?w=500&h=350&fit=crop",
                ["Grand Villa in Alwarpet"] = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=500&h=350&fit=crop",
                ["Luxury Villa in Juhu"] = "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500&h=350&fit=crop",
                
                // Commercial
                ["Office Space in Cyber City"] = "https://images.unsplash.com/photo-1497366754035-f200968a6e72?w=500&h=350&fit=crop",
                ["Retail Space in Phoenix MarketCity"] = "https://images.unsplash.com/photo-1606836576983-8b458e75221d?w=500&h=350&fit=crop",
                ["Warehouse in Bhiwandi"] = "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?w=500&h=350&fit=crop",
                ["IT Office Space in HITEC City"] = "https://images.unsplash.com/photo-1577760258779-e787a1733016?w=500&h=350&fit=crop",
                ["Restaurant Space in Park Street"] = "https://images.unsplash.com/photo-1497215728101-856f4ea42174?w=500&h=350&fit=crop",
                
                // Land
                ["Residential Plot in Sector 150 Noida"] = "https://images.unsplash.com/photo-1500382017468-9049fed747ef?w=500&h=350&fit=crop",
                ["Agricultural Land in Nashik"] = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?w=500&h=350&fit=crop",
                ["Beach Front Land in Alibaug"] = "https://images.unsplash.com/photo-1501084291732-13b1ba8f0ebc?w=500&h=350&fit=crop",
                ["Commercial Land on NH-48"] = "https://images.unsplash.com/photo-1500964757637-c85e8a162699?w=500&h=350&fit=crop",
                ["Hill Station Plot in Mussoorie"] = "https://images.unsplash.com/photo-1500021804447-2ca2eaaaabeb?w=500&h=350&fit=crop"
            };

            var properties = context.Properties.ToList();
            foreach (var property in properties)
            {
                if (originalImages.ContainsKey(property.Title))
                {
                    property.MainImage = originalImages[property.Title];
                    property.UpdatedAt = DateTime.Now;
                    Console.WriteLine($"Restored original image for: {property.Title}");
                }
            }
            
            context.SaveChanges();
            Console.WriteLine($"Restored original images for {properties.Count} properties.");
        }
    }
}