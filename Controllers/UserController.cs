using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

namespace RealEstateManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UserController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private int? GetUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        private bool IsUserLoggedIn()
        {
            return GetUserId() != null;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            // Get dynamic statistics
            var bookingsCount = await _context.Bookings.CountAsync(b => b.UserId == userId);
            var pendingBookings = await _context.Bookings.CountAsync(b => b.UserId == userId && b.Status == "Pending");
            var inquiriesCount = await _context.Inquiries.CountAsync(i => i.UserId == userId);
            var repliedInquiries = await _context.Inquiries.CountAsync(i => i.UserId == userId && i.Status == "Replied");
            var savedProperties = 0; // Can be implemented with a Favorites table later

            // Get recent bookings (last 5)
            var recentBookings = await _context.Bookings
                .Include(b => b.Property)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .Take(5)
                .ToListAsync();

            // Get recent inquiries (last 5)
            var recentInquiries = await _context.Inquiries
                .Include(i => i.Property)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .ToListAsync();

            ViewBag.BookingsCount = bookingsCount;
            ViewBag.PendingBookings = pendingBookings;
            ViewBag.InquiriesCount = inquiriesCount;
            ViewBag.RepliedInquiries = repliedInquiries;
            ViewBag.SavedProperties = savedProperties;
            ViewBag.RecentBookings = recentBookings;
            ViewBag.RecentInquiries = recentInquiries;

            return View(user);
        }

        public async Task<IActionResult> MyBookings()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var bookings = await _context.Bookings
                .Include(b => b.Property)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> BookProperty([FromBody] BookingRequest request)
        {
            if (!IsUserLoggedIn())
            {
                return Json(new { success = false, message = "Please login to book a property" });
            }

            if (request == null || request.PropertyId <= 0)
            {
                return Json(new { success = false, message = "Invalid booking request" });
            }

            var userId = GetUserId();
            var booking = new Booking
            {
                PropertyId = request.PropertyId,
                UserId = userId ?? 0,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                Message = request.Message ?? "",
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Booking request submitted successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || booking.UserId != GetUserId())
            {
                return Json(new { success = false, message = "Booking not found" });
            }

            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Booking cancelled successfully" });
        }

        public async Task<IActionResult> MyInquiries()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var inquiries = await _context.Inquiries
                .Include(i => i.Property)
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(inquiries);
        }

        public async Task<IActionResult> Profile()
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            // Get statistics for profile page
            var bookingsCount = await _context.Bookings.CountAsync(b => b.UserId == userId);
            var inquiriesCount = await _context.Inquiries.CountAsync(i => i.UserId == userId);

            ViewBag.BookingsCount = bookingsCount;
            ViewBag.InquiriesCount = inquiriesCount;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User model, IFormFile? profileImage)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.ZipCode = model.ZipCode;
            user.UpdatedAt = DateTime.Now;

            if (profileImage != null && profileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "users");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                user.ProfileImage = "/images/users/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            if (!IsUserLoggedIn())
            {
                return Json(new { success = false, message = "Please login" });
            }

            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                return Json(new { success = false, message = "Current password is incorrect" });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Password changed successfully!" });
        }
    }
}