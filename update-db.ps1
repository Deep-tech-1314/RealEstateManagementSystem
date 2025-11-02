# PowerShell script to update the database
Write-Host "Starting database update to preserve your image changes..." -ForegroundColor Green

# Run Entity Framework migrations
dotnet ef database update

Write-Host "Database update completed successfully!" -ForegroundColor Green