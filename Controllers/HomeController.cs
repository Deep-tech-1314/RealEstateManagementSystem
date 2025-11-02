using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;
using RealEstateManagementSystem.ViewModels;
using System.Diagnostics;

namespace RealEstateManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch top 3 featured properties for 600x400 cards
            var topProperties = await _context.Properties
                .Where(p => p.IsFeatured && p.Status == "Available")
                .Include(p => p.Owner)
                .OrderByDescending(p => p.ViewCount)
                .Take(3)
                .ToListAsync();

            // Fetch 5 properties from each category
            var allProperties = await _context.Properties
                .Where(p => p.Status == "Available")
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Fetch all active cities
            var cities = await _context.Cities
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.PropertyCount)
                .ToListAsync();

            // Statistics
            ViewBag.TotalProperties = await _context.Properties.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync(u => u.Role == "User");
            ViewBag.FeaturedCount = await _context.Properties.CountAsync(p => p.IsFeatured);
            ViewBag.Cities = cities;
            ViewBag.TopProperties = topProperties;

            return View(allProperties);
        }

        public async Task<IActionResult> Properties([FromQuery] PropertySearchViewModel search)
        {
            // base query - only available listings by default
            var query = _context.Properties
                .Where(p => p.Status == "Available")
                .Include(p => p.Owner)
                .AsQueryable();

            // text search
            if (!string.IsNullOrWhiteSpace(search.SearchTerm))
            {
                var term = search.SearchTerm.Trim();
                query = query.Where(p =>
                    p.Title.Contains(term) ||
                    p.Description.Contains(term) ||
                    p.Address.Contains(term) ||
                    p.City.Contains(term));
            }

            // filters
            if (!string.IsNullOrWhiteSpace(search.PropertyType))
            {
                query = query.Where(p => p.PropertyType == search.PropertyType);
            }

            if (!string.IsNullOrWhiteSpace(search.ListingType))
            {
                query = query.Where(p => p.ListingType == search.ListingType);
            }

            if (!string.IsNullOrWhiteSpace(search.City))
            {
                query = query.Where(p => p.City == search.City);
            }

            if (search.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= search.MinPrice.Value);
            }

            if (search.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= search.MaxPrice.Value);
            }

            if (search.Bedrooms.HasValue)
            {
                query = query.Where(p => p.Bedrooms.HasValue && p.Bedrooms.Value >= search.Bedrooms.Value);
            }

            if (search.Bathrooms.HasValue)
            {
                query = query.Where(p => p.Bathrooms.HasValue && p.Bathrooms.Value >= search.Bathrooms.Value);
            }

            // sorting
            switch (search.SortBy)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "oldest":
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
                case "popular":
                    query = query.OrderByDescending(p => p.ViewCount);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreatedAt); // newest
                    break;
            }

            // totals for pagination
            var totalCount = await query.CountAsync();

            // pagination
            var page = search.Page <= 0 ? 1 : search.Page;
            var pageSize = search.PageSize <= 0 ? 12 : Math.Min(search.PageSize, 48);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // dynamic dropdown data
            var cities = await _context.Properties
                .Where(p => p.Status == "Available")
                .Select(p => p.City)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            var propertyTypes = await _context.Properties
                .Select(p => p.PropertyType)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            var listingTypes = await _context.Properties
                .Select(p => p.ListingType)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            var vm = new PropertyListViewModel
            {
                Properties = items,
                Search = search,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                SortBy = search.SortBy,
                Cities = cities,
                PropertyTypes = propertyTypes,
                ListingTypes = listingTypes
            };

            return View(vm);
        }

        public async Task<IActionResult> PropertyDetails(int id)
        {
            var property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
            {
                return NotFound();
            }

            // Increment view count
            property.ViewCount++;
            await _context.SaveChangesAsync();

            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitInquiry(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.CreatedAt = DateTime.Now;
                inquiry.Status = "New";
                // Ensure inquiry is linked to logged-in user if available
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    inquiry.UserId = userId.Value;
                }
                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Your inquiry has been submitted successfully!";
                return RedirectToAction("PropertyDetails", new { id = inquiry.PropertyId });
            }

            TempData["Error"] = "Failed to submit inquiry. Please try again.";
            return RedirectToAction("PropertyDetails", new { id = inquiry.PropertyId });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.CreatedAt = DateTime.Now;
                inquiry.Status = "New";
                inquiry.PropertyId = null; // General inquiry, not property-specific
                inquiry.UserId = HttpContext.Session.GetInt32("UserId"); // Null if not logged in

                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thank you for contacting us! We'll get back to you soon.";
                return RedirectToAction("Contact");
            }

            return View(inquiry);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}