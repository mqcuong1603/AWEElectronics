# AWE Electronics - Complete Setup Script
# This script performs all setup steps in one go

param(
    [switch]$SkipDatabase,
    [switch]$ForceRecreate
)

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "??????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "?   AWE ELECTRONICS - COMPLETE SETUP WIZARD                 ?" -ForegroundColor Cyan
Write-Host "??????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# Configuration
$CONTAINER_NAME = "sqlserver2022"
$SA_PASSWORD = "YourStrong@Password123"
$SQL_PORT = "1433"
$DATABASE_NAME = "AWEElectronics_DB"
$SQL_SCRIPT = "AWE_Electronics_Database.sql"

# Function to print step
function Write-Step {
    param([string]$Message)
    Write-Host "`n? $Message" -ForegroundColor Yellow
}

# Function to print success
function Write-Success {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Green
}

# Function to print error
function Write-ErrorMsg {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Red
}

# Function to print info
function Write-Info {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Cyan
}

# Step 1: Check Docker
Write-Step "Checking Docker installation..."
try {
    $dockerVersion = docker --version 2>$null
    Write-Success "Docker is installed: $dockerVersion"
} catch {
    Write-ErrorMsg "Docker is not installed or not in PATH"
    Write-Info "Please install Docker Desktop from: https://www.docker.com/products/docker-desktop"
    exit 1
}

# Step 2: Check if Docker is running
Write-Step "Checking if Docker is running..."
try {
    docker ps | Out-Null
    Write-Success "Docker is running"
} catch {
    Write-ErrorMsg "Docker is not running"
    Write-Info "Please start Docker Desktop and try again"
    exit 1
}

# Step 3: Check for existing container
Write-Step "Checking for existing SQL Server container..."
$existingContainer = docker ps -a --filter "name=$CONTAINER_NAME" --format "{{.Names}}" 2>$null

if ($existingContainer -eq $CONTAINER_NAME) {
    Write-Info "Container '$CONTAINER_NAME' already exists"
    
    if ($ForceRecreate) {
        Write-Step "Removing existing container..."
        docker stop $CONTAINER_NAME 2>$null | Out-Null
        docker rm $CONTAINER_NAME 2>$null | Out-Null
        Write-Success "Existing container removed"
    } else {
        $response = Read-Host "Do you want to remove and recreate it? (y/n)"
        if ($response -eq 'y' -or $response -eq 'Y') {
            Write-Step "Removing existing container..."
            docker stop $CONTAINER_NAME 2>$null | Out-Null
            docker rm $CONTAINER_NAME 2>$null | Out-Null
            Write-Success "Existing container removed"
        } else {
            Write-Step "Starting existing container..."
            docker start $CONTAINER_NAME | Out-Null
            Start-Sleep -Seconds 5
            Write-Success "Container started"
            
            if (-not $SkipDatabase) {
                Write-Step "Testing database connection..."
                Start-Sleep -Seconds 5
                & .\test-database-connection.ps1
            }
            exit 0
        }
    }
} else {
    Write-Success "No existing container found"
}

# Step 4: Pull SQL Server image
Write-Step "Pulling SQL Server 2022 Docker image..."
Write-Info "This may take a few minutes on first run..."
docker pull mcr.microsoft.com/mssql/server:2022-latest
if ($LASTEXITCODE -eq 0) {
    Write-Success "Image pulled successfully"
} else {
    Write-ErrorMsg "Failed to pull image"
    exit 1
}

# Step 5: Run SQL Server container
Write-Step "Starting SQL Server container..."
docker run -e "ACCEPT_EULA=Y" `
           -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" `
           -p ${SQL_PORT}:1433 `
           --name $CONTAINER_NAME `
           -d mcr.microsoft.com/mssql/server:2022-latest

if ($LASTEXITCODE -eq 0) {
    Write-Success "SQL Server container started successfully"
} else {
    Write-ErrorMsg "Failed to start SQL Server container"
    exit 1
}

# Step 6: Wait for SQL Server to be ready
Write-Step "Waiting for SQL Server to initialize (30 seconds)..."
for ($i = 30; $i -gt 0; $i--) {
    Write-Progress -Activity "Initializing SQL Server" -Status "Time remaining: $i seconds" -PercentComplete ((30-$i)/30*100)
    Start-Sleep -Seconds 1
}
Write-Progress -Activity "Initializing SQL Server" -Completed
Write-Success "SQL Server should be ready now"

# Step 7: Verify container is running
Write-Step "Verifying container status..."
$containerStatus = docker ps --filter "name=$CONTAINER_NAME" --format "{{.Status}}"
if ($containerStatus) {
    Write-Success "Container is running: $containerStatus"
} else {
    Write-ErrorMsg "Container is not running. Checking logs..."
    docker logs $CONTAINER_NAME
    exit 1
}

# Step 8: Import database schema (if not skipped)
if (-not $SkipDatabase) {
    Write-Step "Importing database schema..."
    
    if (-not (Test-Path $SQL_SCRIPT)) {
        Write-ErrorMsg "Database script not found: $SQL_SCRIPT"
        Write-Info "Please ensure $SQL_SCRIPT is in the current directory"
        exit 1
    }
    
    Write-Info "Copying script to container..."
    docker cp $SQL_SCRIPT ${CONTAINER_NAME}:/tmp/import.sql
    
    Write-Info "Executing database creation script..."
    $sqlOutput = docker exec -i $CONTAINER_NAME /opt/mssql-tools/bin/sqlcmd `
        -S localhost `
        -U sa `
        -P "$SA_PASSWORD" `
        -i /tmp/import.sql 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Success "Database schema imported successfully"
    } else {
        Write-ErrorMsg "Database import had warnings/errors"
        Write-Info "This is often normal if the database already exists"
    }
    
    # Cleanup
    docker exec $CONTAINER_NAME rm /tmp/import.sql 2>$null
}

# Step 9: Test connection
Write-Step "Testing database connection..."
Start-Sleep -Seconds 2

Add-Type -AssemblyName System.Data
$ConnectionString = "Server=localhost,$SQL_PORT;Database=$DATABASE_NAME;User Id=sa;Password=$SA_PASSWORD;TrustServerCertificate=True;"

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = $ConnectionString
    $connection.Open()
    
    Write-Success "Database connection successful!"
    
    # Get database statistics
    $command = $connection.CreateCommand()
    $command.CommandText = @"
SELECT 
    (SELECT COUNT(*) FROM Users) AS Users,
    (SELECT COUNT(*) FROM Categories) AS Categories,
    (SELECT COUNT(*) FROM Products) AS Products,
    (SELECT COUNT(*) FROM Suppliers) AS Suppliers
"@
    $reader = $command.ExecuteReader()
    $reader.Read() | Out-Null
    
    Write-Host ""
    Write-Host "  ?? Database Statistics:" -ForegroundColor Cyan
    Write-Host "     • Users:      $($reader['Users'])" -ForegroundColor White
    Write-Host "     • Categories: $($reader['Categories'])" -ForegroundColor White
    Write-Host "     • Products:   $($reader['Products'])" -ForegroundColor White
    Write-Host "     • Suppliers:  $($reader['Suppliers'])" -ForegroundColor White
    
    $reader.Close()
    $connection.Close()
    
} catch {
    Write-ErrorMsg "Database connection failed: $($_.Exception.Message)"
}

# Display final information
Write-Host ""
Write-Host "??????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host "?              SETUP COMPLETED SUCCESSFULLY!                 ?" -ForegroundColor Green
Write-Host "??????????????????????????????????????????????????????????????" -ForegroundColor Green
Write-Host ""

Write-Host "?? Connection Information:" -ForegroundColor Cyan
Write-Host "   Server:     localhost,$SQL_PORT" -ForegroundColor White
Write-Host "   Database:   $DATABASE_NAME" -ForegroundColor White
Write-Host "   Username:   sa" -ForegroundColor White
Write-Host "   Password:   $SA_PASSWORD" -ForegroundColor White
Write-Host ""

Write-Host "?? Default Login Accounts:" -ForegroundColor Cyan
Write-Host "   ???????????????????????????????????????" -ForegroundColor Gray
Write-Host "   ? Username   ? Password     ? Role    ?" -ForegroundColor Gray
Write-Host "   ???????????????????????????????????????" -ForegroundColor Gray
Write-Host "   ? admin      ? admin123     ? Admin   ?" -ForegroundColor White
Write-Host "   ? manager    ? manager123   ? Manager ?" -ForegroundColor White
Write-Host "   ? staff      ? staff123     ? Staff   ?" -ForegroundColor White
Write-Host "   ???????????????????????????????????????" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Docker Commands:" -ForegroundColor Cyan
Write-Host "   Start:   docker start $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "   Stop:    docker stop $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "   Logs:    docker logs $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "   Status:  docker ps" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Next Steps:" -ForegroundColor Cyan
Write-Host "   1. Open the solution in Visual Studio" -ForegroundColor White
Write-Host "   2. Build the solution (Ctrl+Shift+B)" -ForegroundColor White
Write-Host "   3. Run the application (F5)" -ForegroundColor White
Write-Host "   4. Login with: admin / admin123" -ForegroundColor White
Write-Host ""

Write-Host "?? Documentation:" -ForegroundColor Cyan
Write-Host "   • Full guide: DATABASE_SETUP.md" -ForegroundColor Gray
Write-Host "   • Quick ref:  QUICK_START.md" -ForegroundColor Gray
Write-Host ""

Write-Host "?? You're all set! Happy coding!" -ForegroundColor Green
Write-Host ""
