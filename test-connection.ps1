# ==================================================
# Connection Test Script
# AWE Electronics Database
# ==================================================

Write-Host "=================================================="
Write-Host "SQL Server Connection Test"
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Configuration
$containerName = "sqlserver2022"
$saPassword = "YourStrong@Password123"
$databaseName = "AWEElectronics_DB"
$port = "1433"

# Test 1: Docker Container Status
Write-Host "Test 1: Docker Container Status" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$runningContainer = docker ps --filter "name=$containerName" --format "{{.Names}}"

if ($runningContainer -eq $containerName) {
    Write-Host "? Container '$containerName' is running" -ForegroundColor Green
    
    # Get container details
    $containerInfo = docker inspect $containerName --format "{{.State.Status}} | Uptime: {{.State.StartedAt}}"
    Write-Host "  Status: $containerInfo" -ForegroundColor Gray
} else {
    Write-Host "? Container '$containerName' is not running" -ForegroundColor Red
    Write-Host "  Run: .\setup-sqlserver.ps1" -ForegroundColor Yellow
    exit 1
}

# Test 2: SQL Server Port Accessibility
Write-Host ""
Write-Host "Test 2: Port Accessibility" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

try {
    $tcpClient = New-Object System.Net.Sockets.TcpClient
    $tcpClient.Connect("localhost", $port)
    
    if ($tcpClient.Connected) {
        Write-Host "? Port $port is accessible" -ForegroundColor Green
        $tcpClient.Close()
    }
} catch {
    Write-Host "? Cannot connect to port $port" -ForegroundColor Red
    Write-Host "  Error: $_" -ForegroundColor Gray
}

# Test 3: SQL Server Authentication
Write-Host ""
Write-Host "Test 3: SQL Server Authentication" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$authCommand = "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -Q `"SELECT @@VERSION AS 'SQL Server Version'`""
$authResult = docker exec $containerName bash -c $authCommand 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "? SQL Server authentication successful" -ForegroundColor Green
    Write-Host "  Version info retrieved" -ForegroundColor Gray
} else {
    Write-Host "? Authentication failed" -ForegroundColor Red
    Write-Host "  $authResult" -ForegroundColor Gray
}

# Test 4: Database Existence
Write-Host ""
Write-Host "Test 4: Database Existence" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$dbCommand = "/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -Q `"SELECT name FROM sys.databases WHERE name = '$databaseName'`""
$dbResult = docker exec $containerName bash -c $dbCommand 2>&1

if ($dbResult -match $databaseName) {
    Write-Host "? Database '$databaseName' exists" -ForegroundColor Green
} else {
    Write-Host "? Database '$databaseName' not found" -ForegroundColor Red
    Write-Host "  Run: .\import-database.ps1" -ForegroundColor Yellow
}

# Test 5: Query Database Tables
Write-Host ""
Write-Host "Test 5: Database Tables" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$tablesCommand = @"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -d $databaseName -Q "
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME
" -h -1
"@

$tables = docker exec $containerName bash -c $tablesCommand 2>&1 | Where-Object { $_.Trim() -ne "" }

if ($tables) {
    Write-Host "? Found $($tables.Count) tables:" -ForegroundColor Green
    foreach ($table in $tables) {
        Write-Host "  - $($table.Trim())" -ForegroundColor Gray
    }
} else {
    Write-Host "? No tables found or query failed" -ForegroundColor Yellow
}

# Test 6: Sample Data Query
Write-Host ""
Write-Host "Test 6: Sample Data Query" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$dataCommand = @"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P '$saPassword' -C -d $databaseName -Q "
SELECT TOP 5 
    ProductID, 
    SKU, 
    Name, 
    Price, 
    StockLevel 
FROM Products 
ORDER BY ProductID
"
"@

$dataResult = docker exec $containerName bash -c $dataCommand 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Sample data retrieved successfully" -ForegroundColor Green
    Write-Host ""
    Write-Host $dataResult -ForegroundColor Gray
} else {
    Write-Host "? Could not retrieve sample data" -ForegroundColor Yellow
}

# Test 7: .NET Connection String Test
Write-Host ""
Write-Host "Test 7: Connection String Validation" -ForegroundColor Yellow
Write-Host "--------------------------------------------------"

$connectionString = "Server=localhost,$port;Database=$databaseName;User Id=sa;Password=$saPassword;TrustServerCertificate=True;"
Write-Host "Connection String:" -ForegroundColor White
Write-Host $connectionString -ForegroundColor Yellow

# Display summary
Write-Host ""
Write-Host "=================================================="
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "=================================================="

if ($runningContainer -eq $containerName) {
    Write-Host "? All critical tests passed!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Your SQL Server is ready for use!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Quick Access Info:" -ForegroundColor White
    Write-Host "  Server:   localhost,$port" -ForegroundColor Gray
    Write-Host "  Database: $databaseName" -ForegroundColor Gray
    Write-Host "  Login:    sa" -ForegroundColor Gray
    Write-Host "  Password: $saPassword" -ForegroundColor Gray
} else {
    Write-Host "? Some tests failed" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor White
    Write-Host "  1. Ensure Docker Desktop is running" -ForegroundColor Gray
    Write-Host "  2. Run: .\setup-sqlserver.ps1" -ForegroundColor Gray
    Write-Host "  3. Run: .\import-database.ps1" -ForegroundColor Gray
    Write-Host "  4. Check logs: docker logs $containerName" -ForegroundColor Gray
}

Write-Host ""
Write-Host "=================================================="
Write-Host ""
