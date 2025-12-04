# Import Database Schema Script
# This script imports the AWE Electronics database schema into the running SQL Server container

Write-Host "=== AWE Electronics - Database Schema Import ===" -ForegroundColor Cyan
Write-Host ""

$ContainerName = "sqlserver2022"
$SAPassword = "YourStrong@Password123"
$SQLFile = "AWE_Electronics_Database.sql"

# Check if SQL file exists
if (-not (Test-Path $SQLFile)) {
    Write-Host "? Error: Database script file not found: $SQLFile" -ForegroundColor Red
    Write-Host "Please make sure $SQLFile is in the current directory." -ForegroundColor Yellow
    exit 1
}

Write-Host "Found database script: $SQLFile" -ForegroundColor Green

# Check if container is running
Write-Host "Checking if container '$ContainerName' is running..." -ForegroundColor Yellow

try {
    $containerStatus = docker ps --filter "name=$ContainerName" --format "{{.Names}}"
    
    if ($containerStatus -ne $ContainerName) {
        Write-Host "? Container '$ContainerName' is not running" -ForegroundColor Red
        Write-Host ""
        Write-Host "Please start the container first:" -ForegroundColor Yellow
        Write-Host "  docker start $ContainerName" -ForegroundColor Gray
        Write-Host "  OR run: .\setup-sqlserver-docker.ps1" -ForegroundColor Gray
        exit 1
    }
    
    Write-Host "? Container is running" -ForegroundColor Green
    Write-Host ""
    
} catch {
    Write-Host "? Error checking Docker container" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Copy SQL file to container
Write-Host "Copying database script to container..." -ForegroundColor Yellow
try {
    docker cp $SQLFile ${ContainerName}:/tmp/import.sql
    Write-Host "? Script copied successfully" -ForegroundColor Green
} catch {
    Write-Host "? Failed to copy script to container" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Execute SQL script
Write-Host ""
Write-Host "Importing database schema..." -ForegroundColor Yellow
Write-Host "(This may take 30-60 seconds)" -ForegroundColor Gray
Write-Host ""

try {
    $result = docker exec -i $ContainerName /opt/mssql-tools/bin/sqlcmd `
        -S localhost `
        -U sa `
        -P "$SAPassword" `
        -i /tmp/import.sql
    
    # Display results
    Write-Host $result
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "? Database schema imported successfully!" -ForegroundColor Green
        Write-Host ""
        
        # Verify database
        Write-Host "Verifying database..." -ForegroundColor Yellow
        $verifyQuery = @"
SELECT 
    DB_NAME() AS DatabaseName,
    (SELECT COUNT(*) FROM sys.tables) AS TableCount,
    (SELECT COUNT(*) FROM Users) AS UserCount,
    (SELECT COUNT(*) FROM Products) AS ProductCount
"@
        
        $verifyResult = docker exec -i $ContainerName /opt/mssql-tools/bin/sqlcmd `
            -S localhost `
            -U sa `
            -P "$SAPassword" `
            -d AWEElectronics_DB `
            -Q "$verifyQuery" `
            -h -1 `
            -s "," `
            -W
        
        Write-Host "? Database verification complete" -ForegroundColor Green
        Write-Host ""
        Write-Host "Database Summary:" -ForegroundColor Cyan
        Write-Host $verifyResult
        
    } else {
        Write-Host "? Database import failed" -ForegroundColor Red
        Write-Host "Please check the error messages above" -ForegroundColor Yellow
        exit 1
    }
    
} catch {
    Write-Host "? Error executing SQL script" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Cleanup
Write-Host ""
Write-Host "Cleaning up..." -ForegroundColor Yellow
docker exec $ContainerName rm /tmp/import.sql 2>$null
Write-Host "? Cleanup complete" -ForegroundColor Green

Write-Host ""
Write-Host "=== Setup Complete! ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "You can now:" -ForegroundColor Yellow
Write-Host "  1. Connect with SSMS using:" -ForegroundColor White
Write-Host "     Server: localhost,1433" -ForegroundColor Gray
Write-Host "     Login: sa" -ForegroundColor Gray
Write-Host "     Password: $SAPassword" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Run the test script:" -ForegroundColor White
Write-Host "     .\test-database-connection.ps1" -ForegroundColor Gray
Write-Host ""
Write-Host "  3. Start your application and login with:" -ForegroundColor White
Write-Host "     Username: admin" -ForegroundColor Gray
Write-Host "     Password: admin123" -ForegroundColor Gray
Write-Host ""
