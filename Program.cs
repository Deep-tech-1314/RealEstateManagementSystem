using Microsoft.EntityFrameworkCore;
using RealEstateManagementSystem.Data;
using RealEstateManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Seed Admin User (Run once)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Ensure admin exists with a known password for access
    var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@realestate.com");
    if (admin == null)
    {
        admin = new User
        {
            FullName = "Admin User",
            Email = "admin@realestate.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            PhoneNumber = "1234567890",
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.Now
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
        Console.WriteLine("Admin user created successfully!");
    }
    
    // Restore original images from DatabaseSeeder
    Console.WriteLine("Restoring original property images from DatabaseSeeder...");
    DatabaseSeeder.RestoreOriginalImages(context);
    
    // Reset admin password to a known value to ensure access in development
    admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
    admin.IsActive = true;
    await context.SaveChangesAsync();
    Console.WriteLine("Admin password reset for development access.");

    // Seed Database (if empty)
    if (!context.Properties.Any())
    {
        DatabaseSeeder.SeedData(context);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();