# Database Connection Test Script for AWE Electronics
# This script tests the database connection after setup

Write-Host "=== AWE Electronics - Database Connection Test ===" -ForegroundColor Cyan
Write-Host ""

$ServerName = "localhost,1433"
$DatabaseName = "AWEElectronics_DB"
$Username = "sa"
$Password = "YourStrong@Password123"

$ConnectionString = "Server=$ServerName;Database=$DatabaseName;User Id=$Username;Password=$Password;TrustServerCertificate=True;"

Write-Host "Testing connection to:" -ForegroundColor Yellow
Write-Host "  Server: $ServerName" -ForegroundColor White
Write-Host "  Database: $DatabaseName" -ForegroundColor White
Write-Host ""

try {
    # Load SQL Client assembly
    Add-Type -AssemblyName System.Data
    
    # Create connection
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $ConnectionString
    
    Write-Host "Attempting to connect..." -ForegroundColor Yellow
    $connection.Open()
    
    Write-Host "? Connection successful!" -ForegroundColor Green
    Write-Host ""
    
    # Test query
    $command = $connection.CreateCommand()
    $command.CommandText = @"
SELECT 
    DB_NAME() AS DatabaseName,
    (SELECT COUNT(*) FROM Users) AS TotalUsers,
    (SELECT COUNT(*) FROM Categories) AS TotalCategories,
    (SELECT COUNT(*) FROM Products) AS TotalProducts,
    (SELECT COUNT(*) FROM Suppliers) AS TotalSuppliers
"@
    
    $reader = $command.ExecuteReader()
    
    Write-Host "Database Statistics:" -ForegroundColor Cyan
    while ($reader.Read()) {
        Write-Host "  Database Name:   $($reader['DatabaseName'])" -ForegroundColor White
        Write-Host "  Total Users:     $($reader['TotalUsers'])" -ForegroundColor White
        Write-Host "  Total Categories: $($reader['TotalCategories'])" -ForegroundColor White
        Write-Host "  Total Products:  $($reader['TotalProducts'])" -ForegroundColor White
        Write-Host "  Total Suppliers: $($reader['TotalSuppliers'])" -ForegroundColor White
    }
    $reader.Close()
    
    Write-Host ""
    Write-Host "? Database is ready to use!" -ForegroundColor Green
    
    $connection.Close()
    
} catch {
    Write-Host "? Connection failed!" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Yellow
    Write-Host "  1. Make sure Docker container is running: docker ps" -ForegroundColor Gray
    Write-Host "  2. Check container logs: docker logs sqlserver2022" -ForegroundColor Gray
    Write-Host "  3. Verify SQL Server started: Wait 30 seconds after starting container" -ForegroundColor Gray
    Write-Host "  4. Try restarting container: docker restart sqlserver2022" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
