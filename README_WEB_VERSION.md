# ?? AWE Electronics Web Version - Ready to Run!

## ? TL;DR - Get Running in 2 Minutes

```powershell
# 1. Open PowerShell in solution root
# 2. Run this command:
.\setup-web-version.ps1

# 3. Wait for "SUCCESS" message
# 4. Open AWEElectronics.sln in Visual Studio
# 5. Press F5
# 6. Access: https://localhost:44395/
# 7. Login: admin / admin123
```

**That's it! You're done! ??**

---

## ?? Complete Documentation Package

Your solution now includes everything you need:

### ?? Quick Start
?? **`WEB_VERSION_QUICK_START.md`**
- Step-by-step setup guide
- Troubleshooting section
- Default accounts list
- Testing instructions

### ? Implementation Checklist
?? **`WEB_VERSION_CHECKLIST.md`**
- What has been done
- What you need to do
- Verification steps
- Feature comparison

### ?? Visual Guide
?? **`WEB_VERSION_VISUAL_GUIDE.md`**
- Architecture diagrams
- Setup flow charts
- Connection flow
- Database schema visual

### ?? Deployment Guide
?? **`Deployment/WEB_DEPLOYMENT_GUIDE.md`**
- Azure deployment
- IIS deployment
- Docker deployment
- Production configuration

---

## ?? What Was Changed

### 1. **DatabaseConfig.cs** (Updated)
? Now reads connection string from Web.config
? Maintains backward compatibility
? Falls back to Docker defaults if Web.config not found

```csharp
// Before: Hardcoded connection string
public static string ConnectionString = "Server=localhost...";

// After: Reads from Web.config
public static string ConnectionString
{
    get
    {
        var configConnectionString = ConfigurationManager
            .ConnectionStrings["AWEElectronics"]?.ConnectionString;
        
        return !string.IsNullOrEmpty(configConnectionString)
            ? configConnectionString
            : "Server=localhost,1433;..."; // Fallback
    }
}
```

### 2. **DAL.csproj** (Updated)
? Added `System.Configuration` reference
```xml
<Reference Include="System.Configuration" />
```

### 3. **Web.config.production** (Created)
? Production-ready configuration template
? Configured connection strings
? Authentication settings
? Security headers
? Performance optimizations

### 4. **setup-web-version.ps1** (Created)
? Automated complete setup
? Docker management
? Database import
? Connection testing
? Web.config update

### 5. **Documentation** (Created)
? Quick start guide
? Visual diagrams
? Checklists
? Troubleshooting

---

## ??? Database Features

Your web-optimized database includes:

### Core Tables
- ? **Users** - Staff authentication (Admin, Manager, Staff, etc.)
- ? **Customers** - Customer accounts with email/password
- ? **Addresses** - Multiple shipping/billing addresses per customer
- ? **Products** - Product catalog with SKU, pricing, stock
- ? **Categories** - Hierarchical product categories
- ? **CartItems** - Shopping cart functionality
- ? **Orders** - Order management with status tracking
- ? **OrderDetails** - Order line items
- ? **Payments** - Payment tracking and status
- ? **Suppliers** - Supplier management
- ? **InventoryTransactions** - Stock movement tracking

### Optimized Views
- ? `vw_ProductCatalog` - Product display for web
- ? `vw_CustomerCart` - Shopping cart summary
- ? `vw_OrderSummary` - Order summaries with customer info
- ? `vw_LowStockProducts` - Inventory alerts

### E-commerce Procedures
- ? `sp_AddToCart` - Add products to cart with validation
- ? `sp_CreateOrderFromCart` - Complete checkout process
- ? `sp_UpdateProductStock` - Inventory management

### Sample Data Included
- ? 5 staff users (admin, manager, staff, accountant, agent)
- ? 5 customer accounts with addresses
- ? 16 products across multiple categories
- ? 4 suppliers
- ? 12 product categories

---

## ?? Default Login Credentials

### Staff Access (Backend)
| Username | Password | Role | Use Case |
|----------|----------|------|----------|
| `admin` | `admin123` | Admin | Full system access |
| `manager` | `manager123` | Manager | Store management |
| `staff` | `staff123` | Staff | Order processing |
| `accountant` | `accountant123` | Accountant | Financial reports |
| `agent` | `agent123` | Agent | Customer support |

### Customer Access (Frontend)
| Email | Password |
|-------|----------|
| `alice.nguyen@email.com` | `customer123` |
| `robert.taylor@email.com` | `customer123` |
| `emma.davis@email.com` | `customer123` |
| `michael.wang@email.com` | `customer123` |
| `sophia.martinez@email.com` | `customer123` |

---

## ?? Setup Status

### ? Complete
- [x] Web-optimized database schema created
- [x] DatabaseConfig updated for Web.config support
- [x] System.Configuration reference added
- [x] Production Web.config template created
- [x] Automated setup script created
- [x] Complete documentation written
- [x] Visual guides created
- [x] Troubleshooting guide included

### ? Your Tasks
- [ ] Run `.\setup-web-version.ps1`
- [ ] Open solution in Visual Studio
- [ ] Set Web as startup project
- [ ] Press F5 and test
- [ ] Verify login functionality
- [ ] Test shopping cart features
- [ ] Review deployment options

---

## ?? Verification Checklist

After running the setup, verify:

### Database
```powershell
# Check database exists
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -Q "SELECT name FROM sys.databases WHERE name = 'AWEElectronics_DB'"

# Count products
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -d AWEElectronics_DB -Q "SELECT COUNT(*) FROM Products"
```

Expected: Database exists, 16 products

### Web.config
Check `Web\Web.config` contains:
```xml
<connectionStrings>
  <add name="AWEElectronics" 
       connectionString="Server=localhost,1433;Database=AWEElectronics_DB;..." />
</connectionStrings>
```

### Application
- ? Solution builds without errors
- ? Web app opens in browser
- ? Login page accessible
- ? Can authenticate with test accounts
- ? Products display correctly

---

## ??? Common Commands

### Docker Management
```powershell
# Start SQL Server
docker start sqlserver

# Stop SQL Server
docker stop sqlserver

# View logs
docker logs sqlserver

# Restart SQL Server
docker restart sqlserver

# Remove container (start fresh)
docker rm -f sqlserver
```

### Database Commands
```powershell
# Connect to database
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -d AWEElectronics_DB

# Quick query
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -d AWEElectronics_DB -Q "SELECT * FROM vw_ProductCatalog"

# Re-import schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

### Visual Studio
```
1. Open: AWEElectronics.sln
2. Right-click Web project ? Set as StartUp Project
3. Press F5 (Debug) or Ctrl+F5 (Run without debugging)
4. Access: https://localhost:44395/
```

---

## ?? Setup Script Features

The `setup-web-version.ps1` script does:

1. **? Prerequisite Check**
   - Verifies Docker is running
   - Checks sqlcmd is installed
   
2. **? SQL Server Setup**
   - Creates/starts Docker container
   - Waits for SQL Server to be ready
   - Tests connection
   
3. **? Database Import**
   - Imports web-optimized schema
   - Creates tables, views, procedures
   - Inserts sample data
   
4. **? Configuration**
   - Backs up existing Web.config
   - Copies production template
   - Updates connection string
   
5. **? Build & Verify**
   - Builds solution (if MSBuild available)
   - Tests database connection
   - Generates summary report

**Total time: ~2-3 minutes**

---

## ?? Next Steps

### For Local Development
1. **Run setup:**
   ```powershell
   .\setup-web-version.ps1
   ```

2. **Open and run:**
   - Open `AWEElectronics.sln`
   - Set `Web` as startup
   - Press F5

3. **Test features:**
   - Login as admin
   - Browse products
   - Test shopping cart
   - Create test orders

### For Production Deployment
1. **Choose platform:**
   - Azure App Service (recommended)
   - Windows Server + IIS
   - Docker containers

2. **Review deployment guide:**
   - See `Deployment/WEB_DEPLOYMENT_GUIDE.md`

3. **Update configuration:**
   - Production connection strings
   - HTTPS certificate
   - Security settings

4. **Deploy and monitor:**
   - Deploy application
   - Test functionality
   - Monitor performance

---

## ?? Documentation Files

| File | Description | When to Use |
|------|-------------|-------------|
| `WEB_VERSION_QUICK_START.md` | Quick setup guide | **Start here!** |
| `WEB_VERSION_CHECKLIST.md` | Implementation checklist | Verify what's done |
| `WEB_VERSION_VISUAL_GUIDE.md` | Visual diagrams | Understand architecture |
| `setup-web-version.ps1` | Automated setup | Run first! |
| `Deployment/WEB_DEPLOYMENT_GUIDE.md` | Production deployment | Before going live |
| `Database/AWE_Electronics_WebStore_Schema.sql` | Database schema | Reference |

---

## ?? Tips & Best Practices

### Development
- ? Always use `setup-web-version.ps1` for clean setup
- ? Keep Docker Desktop running during development
- ? Use Visual Studio's built-in debugging (F5)
- ? Review Web.config before each run
- ? Test with multiple user roles

### Database
- ? Backup before making schema changes
- ? Use provided stored procedures for operations
- ? Monitor inventory levels via views
- ? Check sample data for testing scenarios

### Security
- ? Change default passwords before production
- ? Enable HTTPS in production
- ? Use environment-specific connection strings
- ? Implement proper authentication/authorization
- ? Enable audit logging

---

## ?? Troubleshooting Quick Reference

| Problem | Solution |
|---------|----------|
| Cannot connect to database | `docker restart sqlserver` |
| Database doesn't exist | Re-run schema import |
| Port 1433 in use | Use different port (14330) |
| Build errors | Clean and rebuild solution |
| Web.config not found | Copy from Web.config.production |
| Login fails | Verify database has sample users |

**Full troubleshooting guide:** See `WEB_VERSION_QUICK_START.md`

---

## ? Success Criteria

You'll know everything is working when:

- ? Setup script completes with "SUCCESS" message
- ? `SETUP_COMPLETE.txt` file is created
- ? Solution builds without errors (0 errors)
- ? Web app opens automatically in browser
- ? Home page loads without errors
- ? Login page accessible
- ? Can login with admin/admin123
- ? Products list displays correctly
- ? Shopping cart functionality works

---

## ?? You're Ready!

Everything is set up and ready to run. Just execute:

```powershell
.\setup-web-version.ps1
```

Then open Visual Studio, press F5, and you're live! ??

**Need Help?**
- ?? Read: `WEB_VERSION_QUICK_START.md`
- ?? Visual: `WEB_VERSION_VISUAL_GUIDE.md`
- ? Check: `WEB_VERSION_CHECKLIST.md`
- ?? Deploy: `Deployment/WEB_DEPLOYMENT_GUIDE.md`

---

## ?? Support Resources

- **Setup Issues:** See troubleshooting in `WEB_VERSION_QUICK_START.md`
- **Deployment Questions:** See `Deployment/WEB_DEPLOYMENT_GUIDE.md`
- **Database Questions:** Review schema comments in `.sql` file
- **Architecture Questions:** See `WEB_VERSION_VISUAL_GUIDE.md`

---

**Happy Coding! Let's build something amazing! ??**
