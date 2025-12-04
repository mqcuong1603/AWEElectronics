# AWE Electronics Application Startup Checker
# Run this PowerShell script to verify application status

Write-Host "=== AWE Electronics Application Status Checker ===" -ForegroundColor Cyan
Write-Host ""

# Check 1: IIS Express Process
Write-Host "Checking if IIS Express is running..." -ForegroundColor Yellow
$iisProcess = Get-Process -Name iisexpress -ErrorAction SilentlyContinue

if ($iisProcess) {
    Write-Host "? IIS Express is RUNNING" -ForegroundColor Green
    Write-Host "   Process ID: $($iisProcess.Id)" -ForegroundColor Gray
    Write-Host "   Memory Usage: $([math]::Round($iisProcess.WorkingSet64 / 1MB, 2)) MB" -ForegroundColor Gray
} else {
    Write-Host "? IIS Express is NOT RUNNING" -ForegroundColor Red
    Write-Host "   Solution: Open Visual Studio and press F5" -ForegroundColor Yellow
}

Write-Host ""

# Check 2: Port 44395 Listening
Write-Host "Checking if application is listening on port 44395..." -ForegroundColor Yellow
$portCheck = netstat -ano | Select-String "44395" | Select-String "LISTENING"

if ($portCheck) {
    Write-Host "? Port 44395 is LISTENING" -ForegroundColor Green
    Write-Host "   $portCheck" -ForegroundColor Gray
} else {
    Write-Host "? Port 44395 is NOT listening" -ForegroundColor Red
    Write-Host "   Your application might use a different port" -ForegroundColor Yellow
    
    # Try to find any listening ports
    Write-Host ""
    Write-Host "Checking for other active localhost ports..." -ForegroundColor Yellow
    $allPorts = netstat -ano | Select-String "127.0.0.1" | Select-String "LISTENING"
    if ($allPorts) {
        Write-Host "Active localhost ports:" -ForegroundColor Gray
        $allPorts | ForEach-Object { Write-Host "   $_" -ForegroundColor Gray }
    }
}

Write-Host ""

# Check 3: Try HTTP Request
Write-Host "Testing HTTP connection to localhost:44395..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:44395/" -TimeoutSec 2 -UseBasicParsing -ErrorAction Stop
    Write-Host "? Application is RESPONDING" -ForegroundColor Green
    Write-Host "   Status: $($response.StatusCode) $($response.StatusDescription)" -ForegroundColor Gray
    Write-Host "   Content Length: $($response.Content.Length) bytes" -ForegroundColor Gray
} catch {
    Write-Host "? Cannot connect to application" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Gray
    Write-Host "   Solution: Start the application in Visual Studio (F5)" -ForegroundColor Yellow
}

Write-Host ""

# Check 4: SQL Server
Write-Host "Checking SQL Server status..." -ForegroundColor Yellow
$sqlService = Get-Service -Name "MSSQLSERVER" -ErrorAction SilentlyContinue

if ($sqlService) {
    if ($sqlService.Status -eq "Running") {
        Write-Host "? SQL Server is RUNNING" -ForegroundColor Green
    } else {
        Write-Host "? SQL Server is $($sqlService.Status)" -ForegroundColor Red
        Write-Host "   Solution: Start-Service MSSQLSERVER" -ForegroundColor Yellow
    }
} else {
    # Check for SQL Express
    $sqlExpressService = Get-Service -Name "MSSQL`$SQLEXPRESS" -ErrorAction SilentlyContinue
    if ($sqlExpressService) {
        if ($sqlExpressService.Status -eq "Running") {
            Write-Host "? SQL Server Express is RUNNING" -ForegroundColor Green
        } else {
            Write-Host "? SQL Server Express is $($sqlExpressService.Status)" -ForegroundColor Red
            Write-Host "   Solution: Start-Service 'MSSQL`$SQLEXPRESS'" -ForegroundColor Yellow
        }
    } else {
        Write-Host "??  No SQL Server service found" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan

if ($iisProcess -and $portCheck) {
    Write-Host "? APPLICATION IS RUNNING - You can access:" -ForegroundColor Green
    Write-Host "   • Login: http://localhost:44395/" -ForegroundColor White
    Write-Host "   • Diagnostics: http://localhost:44395/Diagnostics/TestConnection" -ForegroundColor White
} else {
    Write-Host "? APPLICATION IS NOT RUNNING" -ForegroundColor Red
    Write-Host ""
    Write-Host "To start the application:" -ForegroundColor Yellow
    Write-Host "1. Open Visual Studio" -ForegroundColor White
    Write-Host "2. Open AWEElectronics.sln" -ForegroundColor White
    Write-Host "3. Set 'Web' as StartUp Project (right-click ? Set as StartUp Project)" -ForegroundColor White
    Write-Host "4. Press F5 (or click green play button)" -ForegroundColor White
    Write-Host "5. Wait for browser to open automatically" -ForegroundColor White
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
