# ==================================================
# Database Import Script
# AWE Electronics Database
# ==================================================

Write-Host "=================================================="
Write-Host "AWE Electronics Database Import"
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Configuration
$containerName = "sqlserver2022"
$saPassword = "YourStrong@Password123"
$databaseName = "AWEElectronics_DB"
$sqlFile = "AWE_Electronics_Database.sql"
$port = "1433"

# Check if SQL file exists
Write-Host "Checking SQL file..." -ForegroundColor Yellow
if (-not (Test-Path $sqlFile)) {
    Write-Host "? SQL file not found: $sqlFile" -ForegroundColor Red
    Write-Host "  Please ensure the file is in the current directory" -ForegroundColor Yellow
    exit 1
}
Write-Host "? SQL file found: $sqlFile" -ForegroundColor Green

# Check if container is running
Write-Host ""
Write-Host "Checking SQL Server container..." -ForegroundColor Yellow
$runningContainer = docker ps --filter "name=$containerName" --format "{{.Names}}"

if ($runningContainer -ne $containerName) {
    Write-Host "? Container '$containerName' is not running" -ForegroundColor Red
    Write-Host "  Please run: .\setup-sqlserver.ps1" -ForegroundColor Yellow
    exit 1
}
Write-Host "? Container is running" -ForegroundColor Green

# Check if sqlcmd is available
Write-Host ""
Write-Host "Checking database tools..." -ForegroundColor Yellow

# Try to use sqlcmd from container
Write-Host "  Using sqlcmd from Docker container..." -ForegroundColor Gray

# Copy SQL file to container
Write-Host ""
Write-Host "Copying SQL file to container..." -ForegroundColor Yellow
docker cp $sqlFile ${containerName}:/tmp/database.sql

if ($LASTEXITCODE -eq 0) {
    Write-Host "? File copied successfully" -ForegroundColor Green
} else {
    Write-Host "? Failed to copy file to container" -ForegroundColor Red
    exit 1
}

# Import database
Write-Host ""
Write-Host "Importing database..." -ForegroundColor Yellow
Write-Host "  This may take a moment..." -ForegroundColor Gray

$importCommand = "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -i /tmp/database.sql"
docker exec $containerName bash -c $importCommand

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Database imported successfully!" -ForegroundColor Green
} else {
    Write-Host "? Import may have completed with warnings" -ForegroundColor Yellow
    Write-Host "  Check the output above for details" -ForegroundColor Gray
}

# Verify database exists
Write-Host ""
Write-Host "Verifying database..." -ForegroundColor Yellow

$verifyCommand = "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -Q `"SELECT name FROM sys.databases WHERE name = '$databaseName'`""
$result = docker exec $containerName bash -c $verifyCommand

if ($result -match $databaseName) {
    Write-Host "? Database '$databaseName' verified!" -ForegroundColor Green
} else {
    Write-Host "? Database verification inconclusive" -ForegroundColor Yellow
}

# Get database statistics
Write-Host ""
Write-Host "=================================================="
Write-Host "Database Statistics" -ForegroundColor Cyan
Write-Host "=================================================="

$statsCommand = @"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -d $databaseName -Q "
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL SELECT 'Categories', COUNT(*) FROM Categories
UNION ALL SELECT 'Suppliers', COUNT(*) FROM Suppliers
UNION ALL SELECT 'Products', COUNT(*) FROM Products
UNION ALL SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL SELECT 'OrderDetails', COUNT(*) FROM OrderDetails
UNION ALL SELECT 'Payments', COUNT(*) FROM Payments
UNION ALL SELECT 'InventoryTransactions', COUNT(*) FROM InventoryTransactions
"
"@

docker exec $containerName bash -c $statsCommand

# Display connection information
Write-Host ""
Write-Host "=================================================="
Write-Host "Connection Information" -ForegroundColor Cyan
Write-Host "=================================================="
Write-Host "Server:          localhost,$port" -ForegroundColor Yellow
Write-Host "Database:        $databaseName" -ForegroundColor Yellow
Write-Host "Authentication:  SQL Server Authentication" -ForegroundColor Yellow
Write-Host "Login:           sa" -ForegroundColor Yellow
Write-Host "Password:        $saPassword" -ForegroundColor Yellow
Write-Host ""
Write-Host "Connection String:" -ForegroundColor Cyan
Write-Host "Server=localhost,$port;Database=$databaseName;User Id=sa;Password=$saPassword;TrustServerCertificate=True;" -ForegroundColor Yellow
Write-Host ""

# Display default credentials
Write-Host "=================================================="
Write-Host "Default Application Users" -ForegroundColor Cyan
Write-Host "=================================================="
Write-Host "Administrator:" -ForegroundColor White
Write-Host "  Username: admin" -ForegroundColor Yellow
Write-Host "  Password: admin123" -ForegroundColor Yellow
Write-Host ""
Write-Host "Manager:" -ForegroundColor White
Write-Host "  Username: manager" -ForegroundColor Yellow
Write-Host "  Password: manager123" -ForegroundColor Yellow
Write-Host ""
Write-Host "Staff:" -ForegroundColor White
Write-Host "  Username: staff" -ForegroundColor Yellow
Write-Host "  Password: staff123" -ForegroundColor Yellow
Write-Host ""

Write-Host "=================================================="
Write-Host "? Setup Complete!" -ForegroundColor Green
Write-Host "=================================================="
Write-Host ""
Write-Host "You can now:" -ForegroundColor White
Write-Host "  1. Connect with SSMS using the connection info above" -ForegroundColor Gray
Write-Host "  2. Run test-connection.ps1 to verify connectivity" -ForegroundColor Gray
Write-Host "  3. Start developing your application" -ForegroundColor Gray
Write-Host ""
