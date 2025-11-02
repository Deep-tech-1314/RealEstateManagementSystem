# Real Estate Management System

A comprehensive ASP.NET Core MVC web application for managing real estate properties, bookings, inquiries, and user interactions.

## Features

- **Property Management**: Browse, search, and view detailed property listings
- **User Authentication**: Secure login and registration system
- **Admin Dashboard**: Complete admin panel for managing properties, users, bookings, and inquiries
- **Property Booking**: Users can book property visits online
- **Inquiry System**: Submit and manage property inquiries
- **Multi-City Support**: Properties available across major Indian cities
- **Property Categories**: Houses, Apartments, Villas, Commercial spaces, and Land
- **Advanced Search**: Filter properties by type, location, price, bedrooms, bathrooms, and more

## Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server (Entity Framework Core)
- **Frontend**: Bootstrap 5, jQuery, Razor Views
- **Authentication**: Session-based authentication with BCrypt password hashing

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full SQL Server instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd RealEstateManagementSystem
   ```

2. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RealEstateDB;Trusted_Connection=true;MultipleActiveResultSets=true"
   }
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. The application will automatically:
   - Create the database if it doesn't exist
   - Run migrations
   - Seed initial data (admin user and sample properties)
   - Launch at `http://localhost:5072` or `https://localhost:7177`

### Default Admin Credentials

- **Email**: `admin@realestate.com`
- **Password**: `Admin@123`

## Project Structure

```
RealEstateManagementSystem/
├── Controllers/         # MVC Controllers (Home, Admin, User, Account)
├── Models/             # Data models and ViewModels
├── Views/              # Razor view files
├── Data/               # DbContext and DatabaseSeeder
├── Services/           # Business logic services
├── Migrations/         # Entity Framework migrations
├── wwwroot/            # Static files (CSS, JS, images)
└── Properties/        # Application settings
```

## Routing

The application is configured with the **Home page (Home/Index) as the default starting point**. 

Default route configuration in `Program.cs`:
```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

## Database

The application uses Entity Framework Core with SQL Server. Migrations are included in the `Migrations/` folder.

To create/update the database:
```bash
dotnet ef database update
```

## License

This project is open source and available for use.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.