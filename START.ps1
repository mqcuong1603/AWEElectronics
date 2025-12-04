# ==================================================
# Quick Start Script - AWE Electronics
# ==================================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   AWE Electronics - Quick Start" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if SQL Server is running
Write-Host "1. Checking SQL Server..." -ForegroundColor Yellow
$container = docker ps --filter "name=sqlserver2022" --format "{{.Names}}"

if ($container -eq "sqlserver2022") {
    Write-Host "   ? SQL Server is running" -ForegroundColor Green
} else {
    Write-Host "   ? SQL Server is not running" -ForegroundColor Yellow
    Write-Host "   Starting SQL Server..." -ForegroundColor Gray
    docker start sqlserver2022
    Start-Sleep -Seconds 5
    Write-Host "   ? SQL Server started" -ForegroundColor Green
}

# Check if database exists
Write-Host ""
Write-Host "2. Checking Database..." -ForegroundColor Yellow
$dbCheck = docker exec sqlserver2022 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -C -Q "SELECT name FROM sys.databases WHERE name = 'AWEElectronics_DB'" -h -1 2>&1

if ($dbCheck -match "AWEElectronics_DB") {
    Write-Host "   ? Database exists" -ForegroundColor Green
} else {
    Write-Host "   ? Database not found!" -ForegroundColor Red
    Write-Host "   Run: .\import-database.ps1" -ForegroundColor Yellow
    exit 1
}

# Check if solution file exists
Write-Host ""
Write-Host "3. Checking Solution..." -ForegroundColor Yellow
if (Test-Path "AWEElectronics.sln") {
    Write-Host "   ? Solution file found" -ForegroundColor Green
} else {
    Write-Host "   ? Solution file not found!" -ForegroundColor Red
    exit 1
}

# Check if Visual Studio is available
Write-Host ""
Write-Host "4. Checking Visual Studio..." -ForegroundColor Yellow

$vsWhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
if (Test-Path $vsWhere) {
    $vsPath = & $vsWhere -latest -property productPath
    if ($vsPath) {
        Write-Host "   ? Visual Studio found" -ForegroundColor Green
        $vsFound = $true
    }
} else {
    Write-Host "   ? Visual Studio not found (will try default path)" -ForegroundColor Yellow
    $vsFound = $false
}

# Display ready message
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "   ? All Prerequisites Ready!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

Write-Host "?? Connection Info:" -ForegroundColor Cyan
Write-Host "   Server:   localhost,1433"
Write-Host "   Database: AWEElectronics_DB"
Write-Host ""

Write-Host "?? Login Credentials:" -ForegroundColor Cyan
Write-Host "   Username: admin"
Write-Host "   Password: admin123"
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   Opening Visual Studio..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Try to open in Visual Studio
if ($vsFound -and $vsPath) {
    Write-Host "Opening solution in Visual Studio..." -ForegroundColor Yellow
    Start-Process $vsPath -ArgumentList "AWEElectronics.sln"
    Write-Host "? Visual Studio opened!" -ForegroundColor Green
} else {
    Write-Host "Opening solution with default program..." -ForegroundColor Yellow
    Start-Process "AWEElectronics.sln"
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   Next Steps in Visual Studio:" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Right-click 'Desktop' project" -ForegroundColor White
Write-Host "   ? Select 'Set as Startup Project'" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Press F5 or click 'Start' button" -ForegroundColor White
Write-Host "   ? Application will launch" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Login with:" -ForegroundColor White
Write-Host "   Username: admin" -ForegroundColor Yellow
Write-Host "   Password: admin123" -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "   Ready to develop! ??" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Keep window open
Write-Host "Press any key to close this window..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
