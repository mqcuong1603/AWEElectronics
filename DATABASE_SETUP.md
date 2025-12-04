# AWE Electronics Database - SQL Server Docker Setup

Complete guide for setting up SQL Server 2022 in Docker and importing the AWE Electronics database.

## ?? Prerequisites

- **Docker Desktop** installed and running
  - Download: https://www.docker.com/products/docker-desktop
- **PowerShell** (Windows) or **PowerShell Core** (cross-platform)
- **(Optional)** SQL Server Management Studio (SSMS) for GUI access
  - Download: https://aka.ms/ssmsfullsetup

## ?? Quick Start (3 Commands)

```powershell
# 1. Setup SQL Server container
.\setup-sqlserver.ps1

# 2. Import database
.\import-database.ps1

# 3. Test connection
.\test-connection.ps1
```

## ?? Detailed Instructions

### Step 1: Setup SQL Server Container

Run the setup script to create and start a SQL Server 2022 container:

```powershell
.\setup-sqlserver.ps1
```

**What this does:**
- ? Checks if Docker is installed and running
- ? Creates a new SQL Server 2022 container (or starts existing one)
- ? Configures port mapping (1433:1433)
- ? Sets up SA account with password
- ? Waits for SQL Server to be ready
- ? Displays connection information

**Expected output:**
```
==================================================
SQL Server 2022 Docker Container Setup
AWE Electronics Database
==================================================

? Docker found: Docker version 24.0.x
? Container created and started successfully
? SQL Server is ready!

Connection Information:
Server:          localhost,1433
Authentication:  SQL Server Authentication
Login:           sa
Password:        YourStrong@Password123
```

### Step 2: Import Database

Import the AWE Electronics database schema and sample data:

```powershell
.\import-database.ps1
```

**What this does:**
- ? Verifies SQL file exists
- ? Copies SQL file to container
- ? Executes database creation script
- ? Verifies database was created
- ? Shows table statistics

**Expected output:**
```
==================================================
AWE Electronics Database Import
==================================================

? SQL file found: AWE_Electronics_Database.sql
? Container is running
? File copied successfully
? Database imported successfully!
? Database 'AWEElectronics_DB' verified!

Database Statistics:
Users                3
Categories          11
Suppliers            3
Products            14
Orders               0
```

### Step 3: Test Connection

Verify everything is working correctly:

```powershell
.\test-connection.ps1
```

**What this does:**
- ? Tests Docker container status
- ? Verifies port accessibility
- ? Tests SQL Server authentication
- ? Checks database existence
- ? Lists all tables
- ? Queries sample data
- ? Validates connection string

## ?? Connection Information

### For SQL Server Management Studio (SSMS)

| Field | Value |
|-------|-------|
| **Server** | `localhost,1433` |
| **Authentication** | SQL Server Authentication |
| **Login** | `sa` |
| **Password** | `YourStrong@Password123` |

### For .NET Applications

```csharp
"Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;"
```

### For Connection Strings (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;"
  }
}
```

## ?? Default Application Users

The database includes three pre-configured users for testing:

| Username | Password | Role | Use Case |
|----------|----------|------|----------|
| `admin` | `admin123` | Admin | Full system access |
| `manager` | `manager123` | Manager | Management functions |
| `staff` | `staff123` | Staff | Basic operations |

> **Security Note:** Change these passwords before deploying to production!

## ?? Database Schema

### Core Tables

- **Users** - User accounts and authentication
- **Categories** - Product categories (hierarchical)
- **Suppliers** - Vendor information
- **Products** - Product catalog with inventory
- **Orders** - Sales order headers
- **OrderDetails** - Order line items
- **Payments** - Payment transactions
- **InventoryTransactions** - Stock movement tracking

### Sample Data Included

- 3 user accounts (admin, manager, staff)
- 11 product categories (electronics, hardware, etc.)
- 3 suppliers
- 14 sample products (CPUs, GPUs, RAM, etc.)

## ?? Docker Management Commands

### Container Operations

```powershell
# View running containers
docker ps

# View all containers (including stopped)
docker ps -a

# Start the container
docker start sqlserver2022

# Stop the container
docker stop sqlserver2022

# Restart the container
docker restart sqlserver2022

# Remove the container (?? deletes all data)
docker rm -f sqlserver2022
```

### Logs and Diagnostics

```powershell
# View container logs
docker logs sqlserver2022

# Follow logs in real-time
docker logs -f sqlserver2022

# Execute SQL query directly
docker exec sqlserver2022 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Password123' -Q "SELECT @@VERSION"

# Access container shell
docker exec -it sqlserver2022 bash
```

## ?? Troubleshooting

### Issue: "Docker is not installed or not running"

**Solution:**
1. Install Docker Desktop from https://www.docker.com/products/docker-desktop
2. Start Docker Desktop
3. Wait for Docker to fully start (whale icon in system tray)
4. Run setup script again

### Issue: "Port 1433 is already in use"

**Solution:**
1. Check if another SQL Server is running:
   ```powershell
   Get-Process | Where-Object {$_.ProcessName -like "*sql*"}
   ```
2. Stop local SQL Server service, or
3. Change port in script (e.g., `-p 1434:1433`)

### Issue: "Login failed for user 'sa'"

**Solution:**
1. Ensure password matches: `YourStrong@Password123`
2. Wait 30 seconds for SQL Server to fully start
3. Check container logs:
   ```powershell
   docker logs sqlserver2022
   ```

### Issue: "Database not found"

**Solution:**
1. Run import script again:
   ```powershell
   .\import-database.ps1
   ```
2. Verify with test script:
   ```powershell
   .\test-connection.ps1
   ```

### Issue: Container keeps stopping

**Solution:**
1. Check container logs for errors:
   ```powershell
   docker logs sqlserver2022
   ```
2. Ensure SA password meets complexity requirements (8+ chars, uppercase, lowercase, numbers, special chars)
3. Check Docker has enough resources (Settings ? Resources)

## ?? Security Best Practices

### For Development

? Current setup is fine for local development

### For Production

?? **DO NOT use default credentials in production!**

1. **Change SA password:**
   ```powershell
   docker exec sqlserver2022 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Password123' -Q "ALTER LOGIN sa WITH PASSWORD = 'NewSecurePassword!123'"
   ```

2. **Create application-specific logins:**
   ```sql
   CREATE LOGIN awe_app_user WITH PASSWORD = 'SecurePassword!123';
   CREATE USER awe_app_user FOR LOGIN awe_app_user;
   GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO awe_app_user;
   ```

3. **Use environment variables for sensitive data**
4. **Enable TLS/SSL encryption**
5. **Implement network isolation**
6. **Regular backups**

## ?? Backup and Restore

### Create Backup

```powershell
docker exec sqlserver2022 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Password123' -Q "BACKUP DATABASE AWEElectronics_DB TO DISK = '/var/opt/mssql/backup/AWEElectronics.bak'"

# Copy backup to host
docker cp sqlserver2022:/var/opt/mssql/backup/AWEElectronics.bak ./AWEElectronics.bak
```

### Restore Backup

```powershell
# Copy backup to container
docker cp ./AWEElectronics.bak sqlserver2022:/var/opt/mssql/backup/

# Restore database
docker exec sqlserver2022 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong@Password123' -Q "RESTORE DATABASE AWEElectronics_DB FROM DISK = '/var/opt/mssql/backup/AWEElectronics.bak' WITH REPLACE"
```

## ?? Additional Resources

- **SQL Server on Docker:** https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker
- **SQL Server Documentation:** https://learn.microsoft.com/en-us/sql/sql-server/
- **Docker Documentation:** https://docs.docker.com/
- **SSMS Download:** https://aka.ms/ssmsfullsetup

## ?? Need Help?

1. **Run diagnostics:**
   ```powershell
   .\test-connection.ps1
   ```

2. **Check container logs:**
   ```powershell
   docker logs sqlserver2022
   ```

3. **Verify Docker status:**
   ```powershell
   docker ps -a
   docker info
   ```

4. **Common issues:** See Troubleshooting section above

## ?? Files in This Directory

| File | Purpose |
|------|---------|
| `AWE_Electronics_Database.sql` | Database schema and sample data |
| `setup-sqlserver.ps1` | Creates SQL Server container |
| `import-database.ps1` | Imports database into SQL Server |
| `test-connection.ps1` | Validates connection and setup |
| `DATABASE_SETUP.md` | This file - complete documentation |

---

**Status:** ? Ready for Development

For production deployment, see the Security Best Practices section.
