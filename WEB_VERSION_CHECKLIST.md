# ? AWE Electronics Web Version - Implementation Checklist

## ?? What Has Been Done

### ? Database Updates
- [x] Created web-optimized database schema (`Database/AWE_Electronics_WebStore_Schema.sql`)
- [x] Added Customers table for user accounts
- [x] Added Addresses table for shipping/billing
- [x] Added CartItems table for shopping cart
- [x] Added stored procedures for e-commerce operations
- [x] Added views for web display (ProductCatalog, CustomerCart)
- [x] Included sample data (5 staff users, 5 customers, 16 products)

### ? Code Updates
- [x] Updated `DAL/DatabaseConfig.cs` to read from Web.config
- [x] Added System.Configuration reference to DAL project
- [x] Maintained backward compatibility with Docker setup

### ? Configuration Files
- [x] Created production-ready `Web/Web.config.production` template
- [x] Configured connection strings section
- [x] Added authentication and security settings
- [x] Configured static content caching
- [x] Added security headers

### ? Setup Scripts
- [x] Created `setup-web-version.ps1` (automated complete setup)
- [x] Includes Docker SQL Server management
- [x] Imports web-optimized schema
- [x] Tests database connection
- [x] Updates Web.config automatically

### ? Documentation
- [x] Created `WEB_VERSION_QUICK_START.md` (step-by-step guide)
- [x] Created `Deployment/WEB_DEPLOYMENT_GUIDE.md` (production deployment)
- [x] Included troubleshooting section
- [x] Listed all default accounts
- [x] Documented deployment options

---

## ?? What You Need to Do Now

### Step 1: Run the Setup Script (2 minutes)

Open PowerShell in solution root:
```powershell
.\setup-web-version.ps1
```

This will:
- ? Start SQL Server in Docker
- ? Import web database schema
- ? Update Web.config
- ? Test connection
- ? Build solution

### Step 2: Open in Visual Studio

1. Open `AWEElectronics.sln`
2. Right-click `Web` project ? **Set as StartUp Project**
3. Press **F5** to run

### Step 3: Test the Application

Access: `https://localhost:44395/`

**Test Login:**
- Admin: `admin` / `admin123`
- Customer: `alice.nguyen@email.com` / `customer123`

---

## ?? Quick Reference

### Connection String (in Web.config)
```xml
<connectionStrings>
  <add name="AWEElectronics" 
       connectionString="Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Docker Commands
```powershell
# Start SQL Server
docker start sqlserver

# Stop SQL Server
docker stop sqlserver

# View logs
docker logs sqlserver

# Remove container (if you need to start fresh)
docker rm -f sqlserver
```

### Test Database
```powershell
# Connect to database
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB

# Count products
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB -Q "SELECT COUNT(*) FROM Products"
```

---

## ?? Verification Steps

### 1. Database Setup ?
```powershell
# Run this to verify database exists with correct schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -Q "SELECT name FROM sys.databases WHERE name = 'AWEElectronics_DB'"
```

Expected: `AWEElectronics_DB`

### 2. Web Config ?
Check that `Web/Web.config` contains:
- ? `<connectionStrings>` section
- ? Connection named "AWEElectronics"
- ? Correct server, database, credentials

### 3. DAL Configuration ?
Verify `DAL/DatabaseConfig.cs`:
- ? Imports `System.Configuration`
- ? Reads from `ConfigurationManager.ConnectionStrings`
- ? Has fallback to default connection

### 4. Build Status ?
In Visual Studio:
- ? Solution builds without errors
- ? No missing references
- ? All projects compile

### 5. Runtime Test ?
Run the application (F5):
- ? Application starts without errors
- ? Home page loads
- ? Can navigate to login page
- ? Can authenticate with test accounts

---

## ?? Feature Comparison

| Feature | Desktop Version | Web Version (NEW) |
|---------|----------------|-------------------|
| Database | Basic schema | ? E-commerce schema |
| Users | Staff only | ? Staff + Customers |
| Cart | N/A | ? Shopping cart |
| Orders | Basic | ? Full order management |
| Addresses | N/A | ? Shipping/billing |
| Payments | Basic | ? Payment tracking |
| Views | N/A | ? Product catalog views |
| Stored Procs | Basic | ? Cart & checkout |

---

## ??? Troubleshooting

### Problem: "Cannot connect to database"
**Solution:**
```powershell
# Restart SQL Server
docker restart sqlserver
Start-Sleep -Seconds 10

# Test connection
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -Q "SELECT 1"
```

### Problem: "Database does not exist"
**Solution:**
```powershell
# Re-import schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

### Problem: "Could not load System.Configuration"
**Solution:**
1. Open `DAL/DAL.csproj`
2. Verify line exists: `<Reference Include="System.Configuration" />`
3. Clean and rebuild solution

### Problem: Port 1433 already in use
**Solution:**
```powershell
# Use different port
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" `
  -p 14330:1433 --name sqlserver `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Update connection string: Server=localhost,14330;...
```

---

## ?? Next Steps

### For Development
- [ ] Run `.\setup-web-version.ps1`
- [ ] Open solution in Visual Studio
- [ ] Set Web as startup project
- [ ] Press F5 and test

### For Production
- [ ] Review `Deployment/WEB_DEPLOYMENT_GUIDE.md`
- [ ] Choose deployment platform (Azure/IIS/Docker)
- [ ] Update connection strings for production
- [ ] Enable HTTPS
- [ ] Configure authentication
- [ ] Set up monitoring

---

## ?? Documentation Files

| File | Purpose |
|------|---------|
| `WEB_VERSION_QUICK_START.md` | Quick start guide (start here!) |
| `Deployment/WEB_DEPLOYMENT_GUIDE.md` | Full deployment guide |
| `Database/AWE_Electronics_WebStore_Schema.sql` | Web database schema |
| `setup-web-version.ps1` | Automated setup script |
| `Web/Web.config.production` | Production config template |

---

## ? Final Checklist

Before running the web version, ensure:

- [ ] Docker Desktop is running
- [ ] No other SQL Server on port 1433
- [ ] Visual Studio 2019+ installed
- [ ] .NET Framework 4.8.1 SDK installed
- [ ] Solution folder has no special characters in path
- [ ] You have internet for NuGet packages (first time)

### Run This Command:
```powershell
.\setup-web-version.ps1
```

### Then:
1. Open solution
2. Set Web as startup
3. Press F5
4. Access `https://localhost:44395/`

---

## ?? Success Indicators

You'll know it's working when:
- ? Script completes without errors
- ? "SETUP COMPLETED SUCCESSFULLY!" message appears
- ? `SETUP_COMPLETE.txt` file is created
- ? Web app opens in browser
- ? You can login with test accounts
- ? Products are displayed

---

**Need Help?**
1. Check `WEB_VERSION_QUICK_START.md`
2. Review troubleshooting section above
3. Verify each verification step
4. Check Docker logs: `docker logs sqlserver`

**You're ready to run the web version! ??**
