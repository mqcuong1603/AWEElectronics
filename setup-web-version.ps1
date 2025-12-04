# ============================================================================
# AWE Electronics - Complete Web Version Setup Script
# ============================================================================
# This script sets up EVERYTHING needed to run the web version
# Run this script from the solution root directory
# ============================================================================

param(
    [switch]$UseWebSchema = $true,
    [switch]$SkipDocker = $false,
    [switch]$UpdateWebConfig = $true
)

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "  AWE Electronics - Complete Web Version Setup" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"
$startTime = Get-Date

# ============================================================================
# STEP 1: Verify Prerequisites
# ============================================================================
Write-Host "[1/6] Verifying prerequisites..." -ForegroundColor Yellow

# Check Docker
if (-not $SkipDocker) {
    Write-Host "  Checking Docker..." -NoNewline
    try {
        $dockerVersion = docker --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host " OK" -ForegroundColor Green
            Write-Host "    $dockerVersion" -ForegroundColor Gray
        } else {
            throw "Docker not found"
        }
    } catch {
        Write-Host " FAILED" -ForegroundColor Red
        Write-Host "    Docker is not installed or not running" -ForegroundColor Red
        Write-Host "    Please install Docker Desktop and try again" -ForegroundColor Yellow
        exit 1
    }
} else {
    Write-Host "  Skipping Docker check (manual SQL Server setup)" -ForegroundColor Yellow
}

# Check sqlcmd
Write-Host "  Checking sqlcmd..." -NoNewline
try {
    $sqlcmdVersion = sqlcmd -? 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host " OK" -ForegroundColor Green
    } else {
        throw "sqlcmd not found"
    }
} catch {
    Write-Host " FAILED" -ForegroundColor Red
    Write-Host "    sqlcmd is not installed" -ForegroundColor Red
    Write-Host "    Please install SQL Server Command Line Tools" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# ============================================================================
# STEP 2: Start SQL Server (Docker)
# ============================================================================
if (-not $SkipDocker) {
    Write-Host "[2/6] Starting SQL Server in Docker..." -ForegroundColor Yellow
    
    # Check if container exists
    $containerExists = docker ps -a --filter "name=sqlserver" --format "{{.Names}}" 2>$null
    
    if ($containerExists -eq "sqlserver") {
        Write-Host "  Container 'sqlserver' already exists" -ForegroundColor Gray
        
        # Check if running
        $containerRunning = docker ps --filter "name=sqlserver" --format "{{.Names}}" 2>$null
        
        if ($containerRunning -eq "sqlserver") {
            Write-Host "  Container is already running" -ForegroundColor Green
        } else {
            Write-Host "  Starting existing container..." -NoNewline
            docker start sqlserver | Out-Null
            Write-Host " OK" -ForegroundColor Green
        }
    } else {
        Write-Host "  Creating new SQL Server container..." -NoNewline
        docker run -e "ACCEPT_EULA=Y" `
                   -e "SA_PASSWORD=YourStrong@Password123" `
                   -e "MSSQL_PID=Express" `
                   -p 1433:1433 `
                   --name sqlserver `
                   -d mcr.microsoft.com/mssql/server:2022-latest | Out-Null
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host " OK" -ForegroundColor Green
        } else {
            Write-Host " FAILED" -ForegroundColor Red
            exit 1
        }
    }
    
    # Wait for SQL Server to be ready
    Write-Host "  Waiting for SQL Server to be ready..." -NoNewline
    $maxAttempts = 30
    $attempt = 0
    $ready = $false
    
    while (-not $ready -and $attempt -lt $maxAttempts) {
        Start-Sleep -Seconds 2
        $attempt++
        
        try {
            $testResult = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -Q "SELECT 1" -h -1 2>$null
            if ($LASTEXITCODE -eq 0) {
                $ready = $true
            }
        } catch {
            # Continue waiting
        }
    }
    
    if ($ready) {
        Write-Host " OK (ready after $($attempt * 2) seconds)" -ForegroundColor Green
    } else {
        Write-Host " TIMEOUT" -ForegroundColor Red
        Write-Host "    SQL Server did not become ready in time" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "[2/6] Skipping Docker setup (manual SQL Server)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# STEP 3: Import Database Schema
# ============================================================================
Write-Host "[3/6] Importing database schema..." -ForegroundColor Yellow

if ($UseWebSchema) {
    $schemaFile = "Database\AWE_Electronics_WebStore_Schema.sql"
    Write-Host "  Using WEB-OPTIMIZED schema (with e-commerce features)" -ForegroundColor Cyan
} else {
    $schemaFile = "AWE_Electronics_Database.sql"
    Write-Host "  Using basic schema" -ForegroundColor Gray
}

if (-not (Test-Path $schemaFile)) {
    Write-Host "  ERROR: Schema file not found: $schemaFile" -ForegroundColor Red
    exit 1
}

Write-Host "  Importing schema from: $schemaFile" -ForegroundColor Gray
Write-Host "  This may take a few moments..." -NoNewline

try {
    $result = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -i $schemaFile -h -1 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host " OK" -ForegroundColor Green
        
        # Show completion messages
        $completionMessages = $result | Where-Object { $_ -match "Database|Users|Sample Data|created|ready" }
        if ($completionMessages) {
            Write-Host "  Database setup completed:" -ForegroundColor Gray
            $completionMessages | ForEach-Object {
                if ($_ -match '\S') {
                    Write-Host "    $_" -ForegroundColor DarkGray
                }
            }
        }
    } else {
        Write-Host " FAILED" -ForegroundColor Red
        Write-Host "  Error output:" -ForegroundColor Red
        $result | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
        exit 1
    }
} catch {
    Write-Host " FAILED" -ForegroundColor Red
    Write-Host "  Error: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# ============================================================================
# STEP 4: Test Database Connection
# ============================================================================
Write-Host "[4/6] Testing database connection..." -ForegroundColor Yellow

try {
    $testQuery = "SELECT DB_NAME() AS DatabaseName, @@VERSION AS SQLVersion, GETDATE() AS CurrentTime"
    $testResult = sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB -Q $testQuery -h -1 -W 2>$null
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  Database connection: " -NoNewline
        Write-Host "SUCCESSFUL" -ForegroundColor Green
        Write-Host "  Database: AWEElectronics_DB" -ForegroundColor Gray
    } else {
        Write-Host "  Database connection: " -NoNewline
        Write-Host "FAILED" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "  Database connection: " -NoNewline
    Write-Host "FAILED" -ForegroundColor Red
    Write-Host "  Error: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""

# ============================================================================
# STEP 5: Update Web.config (if needed)
# ============================================================================
if ($UpdateWebConfig) {
    Write-Host "[5/6] Updating Web.config..." -ForegroundColor Yellow
    
    $webConfigPath = "Web\Web.config"
    $webConfigProductionPath = "Web\Web.config.production"
    
    if (Test-Path $webConfigProductionPath) {
        Write-Host "  Found production Web.config template" -ForegroundColor Gray
        Write-Host "  Backing up current Web.config..." -NoNewline
        
        if (Test-Path $webConfigPath) {
            $backupPath = "$webConfigPath.backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
            Copy-Item $webConfigPath $backupPath -Force
            Write-Host " OK" -ForegroundColor Green
            Write-Host "    Backup saved to: $backupPath" -ForegroundColor Gray
        } else {
            Write-Host " SKIPPED (no existing file)" -ForegroundColor Yellow
        }
        
        Write-Host "  Copying production Web.config..." -NoNewline
        Copy-Item $webConfigProductionPath $webConfigPath -Force
        Write-Host " OK" -ForegroundColor Green
        Write-Host ""
        Write-Host "  IMPORTANT: Review and update the connection string in Web.config" -ForegroundColor Yellow
        Write-Host "  if you're not using the default Docker setup" -ForegroundColor Yellow
    } else {
        Write-Host "  Production Web.config template not found" -ForegroundColor Yellow
        Write-Host "  Please manually update Web\Web.config with connection string" -ForegroundColor Yellow
    }
} else {
    Write-Host "[5/6] Skipping Web.config update" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# STEP 6: Build Solution
# ============================================================================
Write-Host "[6/6] Building solution..." -ForegroundColor Yellow

# Find solution file
$solutionFile = Get-ChildItem -Filter "*.sln" | Select-Object -First 1

if ($solutionFile) {
    Write-Host "  Found solution: $($solutionFile.Name)" -ForegroundColor Gray
    Write-Host "  Building..." -NoNewline
    
    try {
        # Try to use MSBuild
        $msbuild = Get-Command msbuild -ErrorAction SilentlyContinue
        if ($msbuild) {
            & msbuild $solutionFile.FullName /p:Configuration=Debug /v:minimal /nologo 2>&1 | Out-Null
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host " OK" -ForegroundColor Green
            } else {
                Write-Host " FAILED" -ForegroundColor Red
                Write-Host "  Please build the solution manually in Visual Studio" -ForegroundColor Yellow
            }
        } else {
            Write-Host " SKIPPED" -ForegroundColor Yellow
            Write-Host "  MSBuild not found in PATH" -ForegroundColor Gray
            Write-Host "  Please build the solution manually in Visual Studio" -ForegroundColor Yellow
        }
    } catch {
        Write-Host " FAILED" -ForegroundColor Red
        Write-Host "  Error: $_" -ForegroundColor Red
        Write-Host "  Please build the solution manually in Visual Studio" -ForegroundColor Yellow
    }
} else {
    Write-Host "  No solution file found" -ForegroundColor Yellow
    Write-Host "  Please build the project manually in Visual Studio" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# SUMMARY
# ============================================================================
$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host "============================================================================" -ForegroundColor Green
Write-Host "  SETUP COMPLETED SUCCESSFULLY!" -ForegroundColor Green
Write-Host "============================================================================" -ForegroundColor Green
Write-Host ""
Write-Host "Setup completed in $([math]::Round($duration.TotalSeconds, 1)) seconds" -ForegroundColor Gray
Write-Host ""
Write-Host "Database Details:" -ForegroundColor Cyan
Write-Host "  Server:   localhost,1433" -ForegroundColor White
Write-Host "  Database: AWEElectronics_DB" -ForegroundColor White
Write-Host "  User:     sa" -ForegroundColor White
Write-Host "  Password: YourStrong@Password123" -ForegroundColor White
Write-Host ""

if ($UseWebSchema) {
    Write-Host "Default Accounts (Web Schema):" -ForegroundColor Cyan
    Write-Host "  Staff Users:" -ForegroundColor White
    Write-Host "    admin / admin123       (Admin)" -ForegroundColor Gray
    Write-Host "    manager / manager123   (Manager)" -ForegroundColor Gray
    Write-Host "    staff / staff123       (Staff)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  Customer Users:" -ForegroundColor White
    Write-Host "    alice.nguyen@email.com / customer123" -ForegroundColor Gray
    Write-Host "    robert.taylor@email.com / customer123" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "  1. Open the solution in Visual Studio" -ForegroundColor White
Write-Host "  2. Set 'Web' as the startup project" -ForegroundColor White
Write-Host "  3. Review Web\Web.config (connection string)" -ForegroundColor White
Write-Host "  4. Press F5 to run the web application" -ForegroundColor White
Write-Host "  5. The app will open at https://localhost:44395/" -ForegroundColor White
Write-Host ""
Write-Host "Documentation:" -ForegroundColor Cyan
Write-Host "  - Deployment\WEB_DEPLOYMENT_GUIDE.md" -ForegroundColor White
Write-Host "  - Database\AWE_Electronics_WebStore_Schema.sql" -ForegroundColor White
Write-Host ""
Write-Host "============================================================================" -ForegroundColor Green
Write-Host ""

# Create a summary file
$summaryFile = "SETUP_COMPLETE.txt"
$summaryContent = @"
============================================================================
AWE Electronics Web Version - Setup Complete
============================================================================
Date: $(Get-Date -Format "yyyy-MM-DD HH:mm:ss")
Duration: $([math]::Round($duration.TotalSeconds, 1)) seconds

DATABASE CONNECTION
-------------------
Server:   localhost,1433
Database: AWEElectronics_DB
User:     sa
Password: YourStrong@Password123

DEFAULT ACCOUNTS
----------------
Staff:
  - admin / admin123 (Admin)
  - manager / manager123 (Manager)
  - staff / staff123 (Staff)

$(if ($UseWebSchema) {
@"
Customers:
  - alice.nguyen@email.com / customer123
  - robert.taylor@email.com / customer123
"@
})

NEXT STEPS
----------
1. Open solution in Visual Studio
2. Set 'Web' as startup project
3. Review Web\Web.config
4. Press F5 to run
5. Access at https://localhost:44395/

FILES TO REVIEW
---------------
- Web\Web.config (connection string)
- Deployment\WEB_DEPLOYMENT_GUIDE.md
- Database\AWE_Electronics_WebStore_Schema.sql

USEFUL COMMANDS
---------------
Stop SQL Server:  docker stop sqlserver
Start SQL Server: docker start sqlserver
Remove Container: docker rm -f sqlserver

============================================================================
"@

$summaryContent | Out-File -FilePath $summaryFile -Encoding UTF8
Write-Host "Setup summary saved to: $summaryFile" -ForegroundColor Gray
Write-Host ""
