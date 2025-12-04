# ?? AWE Electronics Web Version - Quick Start Guide

## ? Prerequisites Checklist

Before starting, ensure you have:
- [ ] **Docker Desktop** installed and running
- [ ] **Visual Studio 2019+** (with ASP.NET workload)
- [ ] **SQL Server Command Line Tools** (sqlcmd)
- [ ] **Git** (if cloning from repository)

---

## ?? Option 1: Automated Setup (Recommended)

### Run the Complete Setup Script

Open PowerShell in the solution root directory and run:

```powershell
.\setup-web-version.ps1
```

**That's it!** The script will:
1. ? Start SQL Server in Docker
2. ? Import web-optimized database schema
3. ? Configure connection strings
4. ? Test database connection
5. ? Build the solution

**Time required:** ~2-3 minutes

---

## ?? Option 2: Manual Setup (Step-by-Step)

### Step 1: Start SQL Server

```powershell
# Start Docker SQL Server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" `
  -p 1433:1433 --name sqlserver `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Wait 30 seconds for SQL Server to start
Start-Sleep -Seconds 30
```

### Step 2: Import Web Database Schema

```powershell
# Import the web-optimized schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

### Step 3: Update Web.config

1. **Backup current Web.config:**
   ```powershell
   Copy-Item "Web\Web.config" "Web\Web.config.backup"
   ```

2. **Replace with production template:**
   ```powershell
   Copy-Item "Web\Web.config.production" "Web\Web.config"
   ```

3. **Verify connection string in Web\Web.config:**
   ```xml
   <connectionStrings>
     <add name="AWEElectronics" 
          connectionString="Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

### Step 4: Build and Run

1. Open solution in Visual Studio
2. **Set `Web` as startup project:**
   - Right-click Web project ? "Set as StartUp Project"
3. Press **F5** to run
4. Access at: **https://localhost:44395/**

---

## ?? Testing the Setup

### Test Database Connection

```powershell
# Quick connection test
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -d AWEElectronics_DB -Q "SELECT COUNT(*) as ProductCount FROM Products"
```

Expected output: Should show product count (16 products)

### Test Web Application

1. **Navigate to:** `https://localhost:44395/`
2. **Test login with:**
   - **Admin:** username: `admin`, password: `admin123`
   - **Staff:** username: `staff`, password: `staff123`
   - **Customer:** email: `alice.nguyen@email.com`, password: `customer123`

---

## ?? Database Features (Web Schema)

The web-optimized schema includes:

### Tables
- ? **Users** - Staff authentication (Admin, Manager, Staff)
- ? **Customers** - Customer accounts
- ? **Addresses** - Shipping/Billing addresses
- ? **Products** - Product catalog
- ? **Categories** - Product categories
- ? **CartItems** - Shopping cart
- ? **Orders** - Order management
- ? **OrderDetails** - Order line items
- ? **Payments** - Payment tracking
- ? **InventoryTransactions** - Stock movements

### Views
- `vw_ProductCatalog` - Web product display
- `vw_CustomerCart` - Shopping cart view
- `vw_OrderSummary` - Order summary
- `vw_LowStockProducts` - Inventory alerts

### Stored Procedures
- `sp_AddToCart` - Add items to cart
- `sp_CreateOrderFromCart` - Checkout process
- `sp_UpdateProductStock` - Inventory management

---

## ?? Default Accounts

### Staff Users
| Username | Password | Role |
|----------|----------|------|
| `admin` | `admin123` | Admin |
| `manager` | `manager123` | Manager |
| `staff` | `staff123` | Staff |
| `accountant` | `accountant123` | Accountant |
| `agent` | `agent123` | Agent |

### Customer Users
| Email | Password |
|-------|----------|
| `alice.nguyen@email.com` | `customer123` |
| `robert.taylor@email.com` | `customer123` |
| `emma.davis@email.com` | `customer123` |

---

## ??? Troubleshooting

### Issue: "Cannot connect to database"

**Solution:**
```powershell
# 1. Check if SQL Server is running
docker ps

# 2. Restart SQL Server
docker restart sqlserver

# 3. Wait and test connection
Start-Sleep -Seconds 10
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -Q "SELECT 1"
```

### Issue: "Login failed for user 'sa'"

**Solution:**
```powershell
# Password mismatch - reset container
docker rm -f sqlserver
.\setup-web-version.ps1
```

### Issue: "Database 'AWEElectronics_DB' does not exist"

**Solution:**
```powershell
# Re-import schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

### Issue: "Could not load file or assembly 'System.Configuration'"

**Solution:**
1. Clean and rebuild solution
2. Verify `DAL.csproj` has System.Configuration reference:
   ```xml
   <Reference Include="System.Configuration" />
   ```

### Issue: Port 1433 already in use

**Solution:**
```powershell
# Stop existing SQL Server instance
# Option 1: Stop Windows Service
Stop-Service MSSQLSERVER

# Option 2: Use different port
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" `
  -p 14330:1433 --name sqlserver `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Update connection string to: Server=localhost,14330;...
```

---

## ?? File Structure

```
AWEElectronics/
??? Database/
?   ??? AWE_Electronics_WebStore_Schema.sql  ? Use this for web
?   ??? AWE_Electronics_Database.sql          ? Basic schema
??? Web/
?   ??? Web.config                            ? Active config
?   ??? Web.config.production                 ? Production template
?   ??? Controllers/
??? DAL/
?   ??? DatabaseConfig.cs                     ? Updated for Web.config
?   ??? *.DAL.cs
??? BLL/
??? DTO/
??? Desktop/
??? Deployment/
?   ??? WEB_DEPLOYMENT_GUIDE.md
??? setup-web-version.ps1                     ? Run this first!
```

---

## ?? Deployment Options

### Local Development (Current)
- ? SQL Server in Docker
- ? IIS Express (Visual Studio)
- ? Access at `localhost:44395`

### Production Options

1. **Azure (Recommended)**
   - Deploy to Azure App Service
   - Use Azure SQL Database
   - See: `Deployment\WEB_DEPLOYMENT_GUIDE.md`

2. **Windows Server + IIS**
   - Install IIS with ASP.NET 4.8
   - Use local SQL Server
   - Configure SSL certificate

3. **Docker Compose**
   - Containerize web app + database
   - Deploy to any Docker host
   - See deployment guide for docker-compose.yml

---

## ?? Additional Resources

- **Full Deployment Guide:** `Deployment\WEB_DEPLOYMENT_GUIDE.md`
- **Database Schema:** `Database\AWE_Electronics_WebStore_Schema.sql`
- **Architecture:** `ARCHITECTURE.md`

---

## ? Success Checklist

After setup, verify:
- [ ] SQL Server container is running (`docker ps`)
- [ ] Database exists and has data (`SELECT * FROM Products`)
- [ ] Web.config has correct connection string
- [ ] Solution builds without errors
- [ ] Web app opens at `https://localhost:44395/`
- [ ] Can login with admin/admin123
- [ ] Can view products list

---

## ?? You're Ready!

Your AWE Electronics web version is now running!

**Next steps:**
1. Explore the application
2. Test e-commerce features
3. Review deployment guide for production
4. Customize as needed

**Need help?** Check:
- `Deployment\WEB_DEPLOYMENT_GUIDE.md`
- Troubleshooting section above
- GitHub repository issues

---

**Happy Coding! ??**
