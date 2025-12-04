# SQL Server Docker Setup Script for AWE Electronics
# This script automates the setup of SQL Server in Docker

Write-Host "=== AWE Electronics - SQL Server Docker Setup ===" -ForegroundColor Cyan
Write-Host ""

# Configuration
$CONTAINER_NAME = "sqlserver2022"
$SA_PASSWORD = "YourStrong@Password123"
$SQL_PORT = "1433"
$DATABASE_NAME = "AWEElectronics_DB"

# Step 1: Check if Docker is installed
Write-Host "Step 1: Checking Docker installation..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "? Docker is installed: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "? Docker is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install Docker Desktop from: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    exit 1
}

# Step 2: Check if Docker is running
Write-Host "`nStep 2: Checking if Docker is running..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "? Docker is running" -ForegroundColor Green
} catch {
    Write-Host "? Docker is not running" -ForegroundColor Red
    Write-Host "Please start Docker Desktop and try again" -ForegroundColor Yellow
    exit 1
}

# Step 3: Check if container already exists
Write-Host "`nStep 3: Checking for existing SQL Server container..." -ForegroundColor Yellow
$existingContainer = docker ps -a --filter "name=$CONTAINER_NAME" --format "{{.Names}}"
if ($existingContainer -eq $CONTAINER_NAME) {
    Write-Host "Container '$CONTAINER_NAME' already exists" -ForegroundColor Yellow
    $response = Read-Host "Do you want to remove it and create a new one? (y/n)"
    if ($response -eq 'y') {
        Write-Host "Stopping and removing existing container..." -ForegroundColor Yellow
        docker stop $CONTAINER_NAME 2>$null
        docker rm $CONTAINER_NAME 2>$null
        Write-Host "? Existing container removed" -ForegroundColor Green
    } else {
        Write-Host "Using existing container. Starting it if stopped..." -ForegroundColor Yellow
        docker start $CONTAINER_NAME
        Start-Sleep -Seconds 5
        Write-Host "? Container started" -ForegroundColor Green
        exit 0
    }
}

# Step 4: Pull SQL Server image (if not already present)
Write-Host "`nStep 4: Pulling SQL Server 2022 Docker image..." -ForegroundColor Yellow
Write-Host "(This may take a few minutes on first run)" -ForegroundColor Gray
docker pull mcr.microsoft.com/mssql/server:2022-latest
Write-Host "? Image pulled successfully" -ForegroundColor Green

# Step 5: Run SQL Server container
Write-Host "`nStep 5: Starting SQL Server container..." -ForegroundColor Yellow
docker run -e "ACCEPT_EULA=Y" `
           -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" `
           -p ${SQL_PORT}:1433 `
           --name $CONTAINER_NAME `
           -d mcr.microsoft.com/mssql/server:2022-latest

if ($LASTEXITCODE -eq 0) {
    Write-Host "? SQL Server container started successfully" -ForegroundColor Green
} else {
    Write-Host "? Failed to start SQL Server container" -ForegroundColor Red
    exit 1
}

# Step 6: Wait for SQL Server to be ready
Write-Host "`nStep 6: Waiting for SQL Server to be ready (30 seconds)..." -ForegroundColor Yellow
for ($i = 30; $i -gt 0; $i--) {
    Write-Host "  Waiting... $i seconds remaining" -NoNewline -ForegroundColor Gray
    Start-Sleep -Seconds 1
    Write-Host "`r" -NoNewline
}
Write-Host "? SQL Server should be ready now" -ForegroundColor Green

# Step 7: Verify container is running
Write-Host "`nStep 7: Verifying container status..." -ForegroundColor Yellow
$containerStatus = docker ps --filter "name=$CONTAINER_NAME" --format "{{.Status}}"
if ($containerStatus) {
    Write-Host "? Container is running: $containerStatus" -ForegroundColor Green
} else {
    Write-Host "? Container is not running. Checking logs..." -ForegroundColor Red
    docker logs $CONTAINER_NAME
    exit 1
}

# Step 8: Create database
Write-Host "`nStep 8: Creating database '$DATABASE_NAME'..." -ForegroundColor Yellow
$createDbScript = @"
CREATE DATABASE [$DATABASE_NAME];
GO
"@

# Save script to temp file
$tempSqlFile = Join-Path $env:TEMP "create_db.sql"
$createDbScript | Out-File -FilePath $tempSqlFile -Encoding ASCII

# Execute script in container
docker exec -i $CONTAINER_NAME /opt/mssql-tools/bin/sqlcmd `
    -S localhost -U sa -P "$SA_PASSWORD" `
    -Q "CREATE DATABASE [$DATABASE_NAME];"

if ($LASTEXITCODE -eq 0) {
    Write-Host "? Database '$DATABASE_NAME' created successfully" -ForegroundColor Green
} else {
    Write-Host "? Database may already exist or there was an error" -ForegroundColor Yellow
}

# Step 9: Display connection information
Write-Host "`n=== Setup Complete! ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "SQL Server is ready to use with the following settings:" -ForegroundColor Green
Write-Host ""
Write-Host "  Server:     localhost,$SQL_PORT" -ForegroundColor White
Write-Host "  Database:   $DATABASE_NAME" -ForegroundColor White
Write-Host "  Username:   sa" -ForegroundColor White
Write-Host "  Password:   $SA_PASSWORD" -ForegroundColor White
Write-Host ""
Write-Host "Connection String (already configured in DatabaseConfig.cs):" -ForegroundColor Yellow
Write-Host "  Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;" -ForegroundColor Gray
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "  1. Connect with SSMS using the credentials above" -ForegroundColor White
Write-Host "  2. Execute the database schema script (AWE_Electronics_Database.sql)" -ForegroundColor White
Write-Host "  3. Or run: .\create-database-schema.ps1" -ForegroundColor White
Write-Host ""
Write-Host "Useful Docker Commands:" -ForegroundColor Cyan
Write-Host "  Start container:  docker start $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "  Stop container:   docker stop $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "  View logs:        docker logs $CONTAINER_NAME" -ForegroundColor Gray
Write-Host "  Remove container: docker stop $CONTAINER_NAME && docker rm $CONTAINER_NAME" -ForegroundColor Gray
Write-Host ""
