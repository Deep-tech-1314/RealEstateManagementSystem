using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;
using RealEstateManagementSystem.ViewModels;

namespace RealEstateManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // Basic statistics
            ViewBag.TotalUsers = await _context.Users.CountAsync(u => u.Role == "User");
            ViewBag.TotalProperties = await _context.Properties.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalInquiries = await _context.Inquiries.CountAsync();
            ViewBag.PendingBookings = await _context.Bookings.CountAsync(b => b.Status == "Pending");
            ViewBag.NewInquiries = await _context.Inquiries.CountAsync(i => i.Status == "New");
            ViewBag.AvailableProperties = await _context.Properties.CountAsync(p => p.Status == "Available");
            
            // Additional dynamic data
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            
            // New users this month
            ViewBag.NewUsersThisMonth = await _context.Users
                .CountAsync(u => u.CreatedAt.Month == currentMonth && u.CreatedAt.Year == currentYear);
                
            // New properties this month
            ViewBag.NewPropertiesThisMonth = await _context.Properties
                .CountAsync(p => p.CreatedAt.Month == currentMonth && p.CreatedAt.Year == currentYear);
                
            // New bookings this month
            ViewBag.NewBookingsThisMonth = await _context.Bookings
                .CountAsync(b => b.CreatedAt.Month == currentMonth && b.CreatedAt.Year == currentYear);
                
            // New inquiries this month
            ViewBag.NewInquiriesThisMonth = await _context.Inquiries
                .CountAsync(i => i.CreatedAt.Month == currentMonth && i.CreatedAt.Year == currentYear);
                
            // Property type distribution
            ViewBag.HouseCount = await _context.Properties.CountAsync(p => p.PropertyType == "House");
            ViewBag.ApartmentCount = await _context.Properties.CountAsync(p => p.PropertyType == "Apartment");
            ViewBag.VillaCount = await _context.Properties.CountAsync(p => p.PropertyType == "Villa");
            ViewBag.CommercialCount = await _context.Properties.CountAsync(p => p.PropertyType == "Commercial");
            ViewBag.LandCount = await _context.Properties.CountAsync(p => p.PropertyType == "Land");
            
        // Additional stats
        ViewBag.FeaturedCount = await _context.Properties.CountAsync(p => p.IsFeatured);
        ViewBag.TotalViews = await _context.Properties.SumAsync(p => p.ViewCount);
        ViewBag.ActiveUsersToday = await _context.Users.CountAsync(u => u.IsActive);
            
            // Recent activities
            var recentBookings = await _context.Bookings
                .Include(b => b.Property)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToListAsync();
                
            var recentInquiries = await _context.Inquiries
                .Include(i => i.Property)
                .Include(i => i.User)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .ToListAsync();
                
            ViewBag.RecentInquiries = recentInquiries;

            return View(recentBookings);
        }

        // Properties Management
        public async Task<IActionResult> Properties()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var properties = await _context.Properties
                .Include(p => p.Owner)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(properties);
        }

        public IActionResult CreateProperty()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProperty(PropertyViewModel model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var property = new Property
                {
                    Title = model.Title,
                    Description = model.Description,
                    PropertyType = model.PropertyType,
                    ListingType = model.ListingType,
                    Price = model.Price,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Bedrooms = model.Bedrooms,
                    Bathrooms = model.Bathrooms,
                    SquareFeet = model.SquareFeet,
                    YearBuilt = model.YearBuilt,
                    ParkingSpaces = model.ParkingSpaces,
                    Features = model.Features,
                    Status = model.Status,
                    IsFeatured = model.IsFeatured,
                    OwnerId = HttpContext.Session.GetInt32("UserId") ?? 0,
                    CreatedAt = DateTime.Now
                };

                // Handle Main Image
                if (model.MainImage != null && model.MainImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.MainImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.MainImage.CopyToAsync(fileStream);
                    }

                    property.MainImage = "/images/properties/" + uniqueFileName;
                }

                // Handle Additional Images
                if (model.AdditionalImages != null && model.AdditionalImages.Any())
                {
                    var imagesList = new List<string>();
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");

                    foreach (var image in model.AdditionalImages)
                    {
                        if (image.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStream);
                            }

                            imagesList.Add("/images/properties/" + uniqueFileName);
                        }
                    }

                    property.AdditionalImages = string.Join(",", imagesList);
                }

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Property created successfully!";
                return RedirectToAction("Properties");
            }

            return View(model);
        }
        
        // Delete Property
        public async Task<IActionResult> DeleteProperty(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            
            return View(property);
        }
        
        [HttpPost, ActionName("DeleteProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            
            // Delete main image file if exists
            if (!string.IsNullOrEmpty(property.MainImage))
            {
                var mainImagePath = Path.Combine(_environment.WebRootPath, property.MainImage.TrimStart('/'));
                if (System.IO.File.Exists(mainImagePath))
                {
                    System.IO.File.Delete(mainImagePath);
                }
            }
            
            // Delete additional images if they exist
            if (!string.IsNullOrEmpty(property.AdditionalImages))
            {
                var additionalImagePaths = property.AdditionalImages.Split(',');
                foreach (var imagePath in additionalImagePaths)
                {
                    var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }
            
            // Delete related bookings and inquiries
            var relatedBookings = _context.Bookings.Where(b => b.PropertyId == id);
            _context.Bookings.RemoveRange(relatedBookings);
            
            var relatedInquiries = _context.Inquiries.Where(i => i.PropertyId == id);
            _context.Inquiries.RemoveRange(relatedInquiries);
            
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Property deleted successfully!";
            return RedirectToAction(nameof(Properties));
        }
        
        // Cancel Property (Change Status to Cancelled)
        public async Task<IActionResult> CancelProperty(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            
            property.Status = "Cancelled";
            property.UpdatedAt = DateTime.Now;
            
            _context.Update(property);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Property status changed to Cancelled!";
            return RedirectToAction(nameof(Properties));
        }
        
        // Helper method to save images
        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");
            Directory.CreateDirectory(uploadsFolder);
            
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            return "/images/properties/" + uniqueFileName;
        }
        
        // Photo Management Methods
        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int propertyId, string photoPath, bool isMainImage)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" });
            
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return Json(new { success = false, message = "Property not found" });
            
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, photoPath.TrimStart('/'));
                
                if (isMainImage)
                {
                    // Don't delete if it's the default image
                    if (!string.IsNullOrEmpty(property.MainImage) && !property.MainImage.Contains("default-property.jpg"))
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        
                        // Set to default image
                        property.MainImage = "/images/properties/default-property.jpg";
                    }
                }
                else
                {
                    // For additional images
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    
                    // Remove from the comma-separated list
                    if (!string.IsNullOrEmpty(property.AdditionalImages))
                    {
                        var images = property.AdditionalImages.Split(',').ToList();
                        images.Remove(photoPath);
                        property.AdditionalImages = string.Join(",", images);
                    }
                }
                
                property.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPhoto(int propertyId, IFormFile photo, bool isMainImage)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Unauthorized" });
            
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return Json(new { success = false, message = "Property not found" });
            
            if (photo == null || photo.Length == 0)
            {
                return Json(new { success = false, message = "No photo uploaded" });
            }
            
            try
            {
                var photoPath = await SaveImage(photo);
                
                if (isMainImage)
                {
                    // Delete old main image if it exists and is not the default
                    if (!string.IsNullOrEmpty(property.MainImage) && 
                        !property.MainImage.Contains("default-property.jpg"))
                    {
                        var oldPath = Path.Combine(_environment.WebRootPath, property.MainImage.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    
                    property.MainImage = photoPath;
                }
                else
                {
                    // For additional images
                    var images = !string.IsNullOrEmpty(property.AdditionalImages) 
                        ? property.AdditionalImages.Split(',').ToList() 
                        : new List<string>();
                    
                    images.Add(photoPath);
                    property.AdditionalImages = string.Join(",", images);
                }
                
                property.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                
                return Json(new { success = true, photoPath = photoPath });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        
        // We've removed the duplicate AJAX endpoints as they already exist in the controller

        public async Task<IActionResult> EditProperty(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var property = await _context.Properties.FindAsync(id);
            if (property == null) return NotFound();

            var model = new PropertyViewModel
            {
                PropertyId = property.PropertyId,
                Id = property.PropertyId, // Add this line for compatibility
                Title = property.Title,
                Description = property.Description,
                PropertyType = property.PropertyType,
                ListingType = property.ListingType,
                Price = property.Price,
                Address = property.Address,
                City = property.City,
                State = property.State,
                ZipCode = property.ZipCode,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                SquareFeet = property.SquareFeet.HasValue ? (int?)Convert.ToInt32(property.SquareFeet.Value) : null,
                YearBuilt = property.YearBuilt,
                ParkingSpaces = property.ParkingSpaces ?? string.Empty,
                Features = property.Features ?? string.Empty,
                Status = property.Status,
                IsFeatured = property.IsFeatured
            };

            ViewBag.MainImage = property.MainImage;
            ViewBag.AdditionalImages = property.AdditionalImages;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(PropertyViewModel model)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var property = await _context.Properties.FindAsync(model.PropertyId);
            if (property == null) return NotFound();

            property.Title = model.Title;
            property.Description = model.Description;
            property.PropertyType = model.PropertyType;
            property.ListingType = model.ListingType;
            property.Price = model.Price;
            property.Address = model.Address;
            property.City = model.City;
            property.State = model.State;
            property.ZipCode = model.ZipCode;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;
            property.SquareFeet = model.SquareFeet;
            property.YearBuilt = model.YearBuilt;
            property.ParkingSpaces = model.ParkingSpaces;
            property.Features = model.Features;
            property.Status = model.Status;
            property.IsFeatured = model.IsFeatured;
            property.UpdatedAt = DateTime.Now;

            // Handle Main Image
            if (model.MainImage != null && model.MainImage.Length > 0)
            {
                // Delete old image if it exists and is not the default
                if (!string.IsNullOrEmpty(property.MainImage) && 
                    !property.MainImage.Contains("default-property.jpg"))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, property.MainImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.MainImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.MainImage.CopyToAsync(fileStream);
                }

                property.MainImage = "/images/properties/" + uniqueFileName;
            }

            // Handle Additional Images
            if (model.AdditionalImages != null && model.AdditionalImages.Any())
            {
                var imagesList = new List<string>();
                
                // If we already have additional images and want to keep them
                if (!string.IsNullOrEmpty(property.AdditionalImages) && model.KeepExistingImages)
                {
                    imagesList.AddRange(property.AdditionalImages.Split(','));
                }
                else if (!string.IsNullOrEmpty(property.AdditionalImages))
                {
                    // Delete old additional images
                    var oldImagePaths = property.AdditionalImages.Split(',');
                    foreach (var oldImagePath in oldImagePaths)
                    {
                        var fullPath = Path.Combine(_environment.WebRootPath, oldImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "properties");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var image in model.AdditionalImages)
                {
                    if (image.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        imagesList.Add("/images/properties/" + uniqueFileName);
                    }
                }

                property.AdditionalImages = string.Join(",", imagesList);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Property updated successfully!";
            return RedirectToAction("Properties");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePropertyAjax(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.PropertyId == id);
            if (property == null) return Json(new { success = false });

            // Remove dependent bookings and detach inquiries to avoid FK violations
            var bookings = await _context.Bookings.Where(b => b.PropertyId == id).ToListAsync();
            if (bookings.Any())
            {
                _context.Bookings.RemoveRange(bookings);
            }

            var inquiries = await _context.Inquiries.Where(i => i.PropertyId == id).ToListAsync();
            if (inquiries.Any())
            {
                foreach (var inq in inquiries)
                {
                    inq.PropertyId = null;
                }
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var property = await _context.Properties.FindAsync(id);
            if (property == null) return Json(new { success = false });

            property.IsFeatured = !property.IsFeatured;
            property.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isFeatured = property.IsFeatured });
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdatePropertyStatus([FromBody] PropertyStatusUpdateModel model)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var validStatuses = new[] { "Available", "Pending", "Sold", "Cancelled" };
            if (!validStatuses.Contains(model.Status))
            {
                return Json(new { success = false, message = "Invalid status" });
            }

            var property = await _context.Properties.FindAsync(model.Id);
            if (property == null) return Json(new { success = false, message = "Property not found" });

            try
            {
                property.Status = model.Status;
                property.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class PropertyStatusUpdateModel
        {
            public int Id { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        // Bookings Management
        public async Task<IActionResult> Bookings()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var bookings = await _context.Bookings
                .Include(b => b.Property)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status, string adminNotes)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return Json(new { success = false });

            booking.Status = status;
            booking.AdminNotes = adminNotes;
            booking.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return Json(new { success = false, message = "Booking not found" });

            try
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Users Management
        public async Task<IActionResult> Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var users = await _context.Users
                .Where(u => u.Role == "User")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var user = await _context.Users.FindAsync(id);
            if (user == null) return Json(new { success = false });

            user.IsActive = !user.IsActive;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isActive = user.IsActive });
        }

        // Inquiries Management
        public async Task<IActionResult> Inquiries()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var inquiries = await _context.Inquiries
                .Include(i => i.Property)
                .Include(i => i.User)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(inquiries);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyInquiry(int id, string reply)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null) return Json(new { success = false });

            inquiry.AdminReply = reply;
            inquiry.Status = "Replied";
            inquiry.RepliedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}