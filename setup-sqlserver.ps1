# ==================================================
# SQL Server 2022 Docker Setup Script
# AWE Electronics Database
# ==================================================

Write-Host "=================================================="
Write-Host "SQL Server 2022 Docker Container Setup"
Write-Host "AWE Electronics Database"
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Configuration
$containerName = "sqlserver2022"
$saPassword = "YourStrong@Password123"
$port = "1433"
$sqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest"

# Check if Docker is running
Write-Host "Checking Docker status..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "? Docker found: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "? Docker is not installed or not running!" -ForegroundColor Red
    Write-Host "  Please install Docker Desktop from: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    exit 1
}

# Check if container already exists
Write-Host ""
Write-Host "Checking for existing SQL Server container..." -ForegroundColor Yellow
$existingContainer = docker ps -a --filter "name=$containerName" --format "{{.Names}}"

if ($existingContainer -eq $containerName) {
    Write-Host "? Container '$containerName' already exists" -ForegroundColor Green
    
    # Check if it's running
    $runningContainer = docker ps --filter "name=$containerName" --format "{{.Names}}"
    
    if ($runningContainer -eq $containerName) {
        Write-Host "  Container is already running" -ForegroundColor Green
    } else {
        Write-Host "  Starting existing container..." -ForegroundColor Yellow
        docker start $containerName
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ? Container started successfully" -ForegroundColor Green
        } else {
            Write-Host "  ? Failed to start container" -ForegroundColor Red
            exit 1
        }
    }
} else {
    Write-Host "Creating new SQL Server 2022 container..." -ForegroundColor Yellow
    
    # Pull latest image
    Write-Host "  Pulling SQL Server 2022 image..." -ForegroundColor Yellow
    docker pull $sqlServerImage
    
    # Create and run container
    Write-Host "  Creating container '$containerName'..." -ForegroundColor Yellow
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$saPassword" `
        -p ${port}:1433 --name $containerName `
        -d $sqlServerImage
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Container created and started successfully" -ForegroundColor Green
    } else {
        Write-Host "  ? Failed to create container" -ForegroundColor Red
        exit 1
    }
}

# Wait for SQL Server to be ready
Write-Host ""
Write-Host "Waiting for SQL Server to be ready..." -ForegroundColor Yellow
Write-Host "  This may take 30-45 seconds..." -ForegroundColor Gray

$maxAttempts = 15
$attempt = 0
$ready = $false

while ($attempt -lt $maxAttempts -and -not $ready) {
    $attempt++
    Start-Sleep -Seconds 3
    
    # Check if SQL Server is accepting connections
    $logOutput = docker logs $containerName 2>&1 | Select-String "SQL Server is now ready for client connections"
    
    if ($logOutput) {
        $ready = $true
        Write-Host "  ? SQL Server is ready!" -ForegroundColor Green
    } else {
        Write-Host "  Attempt $attempt/$maxAttempts - Still starting..." -ForegroundColor Gray
    }
}

if (-not $ready) {
    Write-Host "  ? SQL Server may still be starting. Check logs with: docker logs $containerName" -ForegroundColor Yellow
}

# Display connection information
Write-Host ""
Write-Host "=================================================="
Write-Host "SQL Server Container Information" -ForegroundColor Cyan
Write-Host "=================================================="
Write-Host "Container Name:  $containerName" -ForegroundColor White
Write-Host "Image:           $sqlServerImage" -ForegroundColor White
Write-Host "Port:            $port" -ForegroundColor White
Write-Host "SA Password:     $saPassword" -ForegroundColor White
Write-Host ""
Write-Host "Connection Strings:" -ForegroundColor Cyan
Write-Host "--------------------------------------------------"
Write-Host "Server:          localhost,$port" -ForegroundColor Yellow
Write-Host "Authentication:  SQL Server Authentication" -ForegroundColor Yellow
Write-Host "Login:           sa" -ForegroundColor Yellow
Write-Host "Password:        $saPassword" -ForegroundColor Yellow
Write-Host ""
Write-Host "Connection String:" -ForegroundColor Cyan
Write-Host "Server=localhost,$port;Database=AWEElectronics_DB;User Id=sa;Password=$saPassword;TrustServerCertificate=True;" -ForegroundColor Yellow
Write-Host ""

# Display next steps
Write-Host "=================================================="
Write-Host "Next Steps" -ForegroundColor Cyan
Write-Host "=================================================="
Write-Host "1. Import Database:" -ForegroundColor White
Write-Host "   Run: .\import-database.ps1" -ForegroundColor Yellow
Write-Host ""
Write-Host "2. Connect with SSMS:" -ForegroundColor White
Write-Host "   Server: localhost,$port" -ForegroundColor Yellow
Write-Host "   Auth:   SQL Server Authentication" -ForegroundColor Yellow
Write-Host "   Login:  sa" -ForegroundColor Yellow
Write-Host "   Pass:   $saPassword" -ForegroundColor Yellow
Write-Host ""
Write-Host "3. Useful Docker Commands:" -ForegroundColor White
Write-Host "   Stop:    docker stop $containerName" -ForegroundColor Gray
Write-Host "   Start:   docker start $containerName" -ForegroundColor Gray
Write-Host "   Remove:  docker rm -f $containerName" -ForegroundColor Gray
Write-Host "   Logs:    docker logs $containerName" -ForegroundColor Gray
Write-Host "=================================================="
Write-Host ""
