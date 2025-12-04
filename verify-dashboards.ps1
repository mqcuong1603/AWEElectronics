# AWE Electronics Dashboard Verification Script
# This script verifies that role-based dashboards are properly configured

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "AWE Electronics Dashboard Verification" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$errors = 0
$warnings = 0

# Check 1: Verify dashboard view files exist
Write-Host "[1/7] Checking dashboard view files..." -ForegroundColor Yellow
$viewPath = "D:\SE\final\AWEElectronics\Web\Views\Home"
$requiredViews = @(
    "Dashboard.cshtml",
    "DashboardAdmin.cshtml",
    "DashboardStaff.cshtml",
    "DashboardAgent.cshtml"
)

foreach ($view in $requiredViews) {
    $filePath = Join-Path $viewPath $view
    if (Test-Path $filePath) {
        $fileInfo = Get-Item $filePath
        Write-Host "  ? $view exists ($($fileInfo.Length) bytes)" -ForegroundColor Green
    } else {
        Write-Host "  ? $view NOT FOUND!" -ForegroundColor Red
        $errors++
    }
}
Write-Host ""

# Check 2: Verify HomeController.cs exists
Write-Host "[2/7] Checking HomeController.cs..." -ForegroundColor Yellow
$controllerPath = "D:\SE\final\AWEElectronics\Web\Controllers\HomeController.cs"
if (Test-Path $controllerPath) {
    $content = Get-Content $controllerPath -Raw
    
    # Check for role-specific view returns
    if ($content -match 'View\("DashboardAdmin"\)') {
        Write-Host "  ? DashboardAdmin view return found" -ForegroundColor Green
    } else {
        Write-Host "  ? DashboardAdmin view return NOT FOUND!" -ForegroundColor Red
        $errors++
    }
    
    if ($content -match 'View\("DashboardStaff"\)') {
        Write-Host "  ? DashboardStaff view return found" -ForegroundColor Green
    } else {
        Write-Host "  ? DashboardStaff view return NOT FOUND!" -ForegroundColor Red
        $errors++
    }
    
    if ($content -match 'View\("DashboardAgent"\)') {
        Write-Host "  ? DashboardAgent view return found" -ForegroundColor Green
    } else {
        Write-Host "  ? DashboardAgent view return NOT FOUND!" -ForegroundColor Red
        $errors++
    }
} else {
    Write-Host "  ? HomeController.cs NOT FOUND!" -ForegroundColor Red
    $errors++
}
Write-Host ""

# Check 3: Verify database connection
Write-Host "[3/7] Checking database connection..." -ForegroundColor Yellow
try {
    $result = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d master -Q "SELECT @@VERSION" -h -1 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? SQL Server is accessible" -ForegroundColor Green
    } else {
        Write-Host "  ? Cannot connect to SQL Server" -ForegroundColor Red
        $errors++
    }
} catch {
    Write-Host "  ? SQL Server connection failed: $($_.Exception.Message)" -ForegroundColor Red
    $errors++
}
Write-Host ""

# Check 4: Verify database exists
Write-Host "[4/7] Checking AWEElectronics_DB database..." -ForegroundColor Yellow
try {
    $result = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB -Q "SELECT 1" -h -1 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? AWEElectronics_DB database exists" -ForegroundColor Green
    } else {
        Write-Host "  ? AWEElectronics_DB database not found" -ForegroundColor Red
        $errors++
    }
} catch {
    Write-Host "  ? Database check failed: $($_.Exception.Message)" -ForegroundColor Red
    $errors++
}
Write-Host ""

# Check 5: Verify users and roles
Write-Host "[5/7] Checking user accounts and roles..." -ForegroundColor Yellow
try {
    $result = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB -Q "SELECT Username, Role, Status FROM Users WHERE Username IN ('admin','jsmith','bwilson')" -W -h -1 2>&1
    
    if ($result -match "admin") {
        if ($result -match "Admin\s+Active") {
            Write-Host "  ? admin user: Role = Admin, Status = Active" -ForegroundColor Green
        } else {
            Write-Host "  ??  admin user: Role or Status may be incorrect" -ForegroundColor Yellow
            $warnings++
        }
    } else {
        Write-Host "  ? admin user not found" -ForegroundColor Red
        $errors++
    }
    
    if ($result -match "jsmith") {
        if ($result -match "Staff\s+Active") {
            Write-Host "  ? jsmith user: Role = Staff, Status = Active" -ForegroundColor Green
        } else {
            Write-Host "  ??  jsmith user: Role or Status may be incorrect" -ForegroundColor Yellow
            $warnings++
        }
    } else {
        Write-Host "  ? jsmith user not found" -ForegroundColor Red
        $errors++
    }
    
    if ($result -match "bwilson") {
        if ($result -match "Agent\s+Active") {
            Write-Host "  ? bwilson user: Role = Agent, Status = Active" -ForegroundColor Green
        } else {
            Write-Host "  ??  bwilson user: Role or Status may be incorrect" -ForegroundColor Yellow
            $warnings++
        }
    } else {
        Write-Host "  ? bwilson user not found" -ForegroundColor Red
        $errors++
    }
} catch {
    Write-Host "  ? User verification failed: $($_.Exception.Message)" -ForegroundColor Red
    $errors++
}
Write-Host ""

# Check 6: Verify IIS Express is running
Write-Host "[6/7] Checking IIS Express status..." -ForegroundColor Yellow
$iisProcess = Get-Process -Name iisexpress -ErrorAction SilentlyContinue
if ($iisProcess) {
    Write-Host "  ? IIS Express is running (PID: $($iisProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "  ??  IIS Express is not running - start app in Visual Studio" -ForegroundColor Yellow
    $warnings++
}
Write-Host ""

# Check 7: Check if port 44395 is listening
Write-Host "[7/7] Checking application port..." -ForegroundColor Yellow
$port = 44395
$listening = Get-NetTCPConnection -LocalPort $port -State Listen -ErrorAction SilentlyContinue
if ($listening) {
    Write-Host "  ? Port $port is listening" -ForegroundColor Green
} else {
    Write-Host "  ??  Port $port is not listening - start app in Visual Studio" -ForegroundColor Yellow
    $warnings++
}
Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "VERIFICATION SUMMARY" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($errors -eq 0 -and $warnings -eq 0) {
    Write-Host "? ALL CHECKS PASSED!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Your role-based dashboards are properly configured!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. If app is not running: Press F5 in Visual Studio" -ForegroundColor White
    Write-Host "2. Clear browser cache: Ctrl+Shift+Delete" -ForegroundColor White
    Write-Host "3. Test each role:" -ForegroundColor White
    Write-Host "   - admin/password123   ? Should see RED dashboard" -ForegroundColor White
    Write-Host "   - jsmith/password123  ? Should see GREEN dashboard" -ForegroundColor White
    Write-Host "   - bwilson/password123 ? Should see YELLOW dashboard" -ForegroundColor White
} elseif ($errors -eq 0) {
    Write-Host "??  PASSED WITH $warnings WARNING(S)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Configuration is OK but some optional components need attention." -ForegroundColor Yellow
    Write-Host "Review warnings above and start the application if needed." -ForegroundColor Yellow
} else {
    Write-Host "? FAILED WITH $errors ERROR(S) AND $warnings WARNING(S)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Critical issues found! Review errors above." -ForegroundColor Red
    Write-Host "Fix the errors before testing the dashboards." -ForegroundColor Red
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Pause so user can read results
Read-Host "Press Enter to exit"
