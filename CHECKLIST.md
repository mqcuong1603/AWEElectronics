# AWE Electronics - Setup Checklist

## ? Pre-Setup Checklist

Before you begin, make sure you have:

- [ ] **Windows 10/11** (64-bit) or **Windows Server 2016+**
- [ ] **4GB RAM minimum** (8GB recommended)
- [ ] **10GB free disk space**
- [ ] **Administrator privileges** on your computer
- [ ] **Internet connection** (for downloading Docker image)

## ?? Software Installation Checklist

### 1. Docker Desktop
- [ ] Downloaded Docker Desktop from https://www.docker.com/products/docker-desktop
- [ ] Installed Docker Desktop
- [ ] Started Docker Desktop
- [ ] Verified Docker is running (green icon in system tray)
- [ ] Tested Docker: Opened PowerShell and ran `docker --version`

### 2. SQL Server Management Studio (Optional but Recommended)
- [ ] Downloaded SSMS from https://aka.ms/ssmsfullsetup
- [ ] Installed SSMS
- [ ] Can launch SSMS from Start menu

### 3. Visual Studio
- [ ] Have Visual Studio 2019 or newer installed
- [ ] .NET Framework 4.8.1 development tools installed

## ?? Database Setup Checklist

### Method 1: Automated Setup (Recommended)

- [ ] Opened PowerShell **as Administrator**
- [ ] Navigated to project directory: `cd D:\SE\final\AWEElectronics`
- [ ] Ran automated setup: `.\complete-setup.ps1`
- [ ] Waited for completion (2-3 minutes)
- [ ] Saw "SETUP COMPLETED SUCCESSFULLY!" message
- [ ] Verified connection info displayed correctly

### Method 2: Manual Setup (Alternative)

- [ ] Ran: `.\setup-sqlserver-docker.ps1`
- [ ] Waited 30 seconds after container started
- [ ] Ran: `.\import-database-schema.ps1` (if needed)
- [ ] Ran: `.\test-database-connection.ps1`
- [ ] Connection test passed ?

## ?? Verification Checklist

### Docker Container
- [ ] Container is running: `docker ps` shows `sqlserver2022`
- [ ] Container status is "Up" (not "Exited")
- [ ] Port 1433 is mapped correctly
- [ ] No errors in logs: `docker logs sqlserver2022`

### Database Connection
- [ ] Can connect with test script: `.\test-database-connection.ps1`
- [ ] Connection successful message appeared
- [ ] Database statistics showed:
  - [ ] 3 Users
  - [ ] 11 Categories
  - [ ] 14 Products
  - [ ] 3 Suppliers

### SSMS Connection (If using SSMS)
- [ ] Opened SQL Server Management Studio
- [ ] Used connection info:
  - Server: `localhost,1433`
  - Authentication: SQL Server Authentication
  - Login: `sa`
  - Password: `YourStrong@Password123`
- [ ] Successfully connected
- [ ] Can see `AWEElectronics_DB` database
- [ ] Can expand database and see tables

### Database Tables
- [ ] Users table exists and has 3 records
- [ ] Categories table exists and has 11 records
- [ ] Products table exists and has 14 records
- [ ] Suppliers table exists and has 3 records
- [ ] Orders table exists
- [ ] OrderDetails table exists
- [ ] Payments table exists
- [ ] InventoryTransactions table exists

## ??? Application Setup Checklist

### Opening the Solution
- [ ] Opened Visual Studio
- [ ] Opened solution file: `AWEElectronics.sln`
- [ ] Solution loaded without errors
- [ ] Can see all projects in Solution Explorer:
  - [ ] BLL (Business Logic Layer)
  - [ ] DAL (Data Access Layer)
  - [ ] DTO (Data Transfer Objects)
  - [ ] Desktop (WinForms Application)
  - [ ] Web (MVC Web Application)

### Building the Solution
- [ ] Right-clicked solution ? "Restore NuGet Packages"
- [ ] Waited for packages to restore
- [ ] Pressed Ctrl+Shift+B to build
- [ ] Build succeeded with 0 errors
- [ ] No critical warnings

### Database Configuration
- [ ] Opened `DAL\DatabaseConfig.cs`
- [ ] Verified connection string matches:
  ```csharp
  Server=localhost,1433;
  Database=AWEElectronics_DB;
  User Id=sa;
  Password=YourStrong@Password123;
  TrustServerCertificate=True;
  ```
- [ ] Connection string is correct

## ?? Running the Application Checklist

### Desktop Application
- [ ] Set Desktop project as startup project (right-click ? Set as Startup Project)
- [ ] Pressed F5 to run
- [ ] Application launched successfully
- [ ] Login screen appeared
- [ ] Logged in with: `admin` / `admin123`
- [ ] Main dashboard appeared
- [ ] Can navigate through menus:
  - [ ] Products
  - [ ] Inventory
  - [ ] Orders
  - [ ] Reports
  - [ ] Users

### Web Application
- [ ] Set Web project as startup project
- [ ] Pressed F5 to run
- [ ] Browser opened automatically
- [ ] Website loaded successfully
- [ ] Can browse products
- [ ] Can view categories

## ?? Functionality Testing Checklist

### User Management
- [ ] Can view users list
- [ ] Can see 3 default users (admin, manager, staff)
- [ ] User roles display correctly
- [ ] Can add new user (if permissions allow)

### Product Management
- [ ] Can view products list
- [ ] Products display with prices and stock levels
- [ ] Can see 14 sample products
- [ ] Categories are linked correctly
- [ ] Can search for products
- [ ] Low stock products are highlighted

### Inventory
- [ ] Can view current stock levels
- [ ] Stock level warnings show (if any)
- [ ] Can view inventory transactions

### Orders (If testing sales)
- [ ] Can create a new order
- [ ] Can add products to order
- [ ] Quantities are validated
- [ ] Stock levels update after order
- [ ] Order total calculates correctly

## ?? Data Verification Checklist

### Sample Users
- [ ] admin user exists (Role: Admin)
- [ ] manager user exists (Role: Manager)
- [ ] staff user exists (Role: Staff)
- [ ] Passwords work for all users

### Sample Categories
- [ ] Electronics (parent category)
- [ ] Computer Hardware (sub-category)
- [ ] Smartphones (sub-category)
- [ ] Audio Equipment (sub-category)
- [ ] And 7 more sub-categories

### Sample Products
- [ ] AMD Ryzen 5 5600X (CPU)
- [ ] Intel Core i5-12600K (CPU)
- [ ] NVIDIA RTX 3060 Ti (GPU)
- [ ] AMD Radeon RX 6700 XT (GPU)
- [ ] Corsair Vengeance 16GB (RAM)
- [ ] Samsung 980 Pro 1TB (SSD)
- [ ] And 8 more products
- [ ] All products have prices > 0
- [ ] All products have stock levels > 0

### Sample Suppliers
- [ ] Tech Distributors Inc
- [ ] Global Electronics Co
- [ ] Premium Components Ltd
- [ ] All suppliers have contact information

## ?? Security Checklist

### Password Security
- [ ] Default passwords are documented
- [ ] Understand that default passwords are for DEVELOPMENT ONLY
- [ ] Plan to change passwords for production
- [ ] Know how to change SA password (see DATABASE_SETUP.md)

### Access Control
- [ ] Understand role-based access (Admin, Manager, Staff)
- [ ] Know that admin has full access
- [ ] Manager and Staff have limited access
- [ ] Plan to create proper user accounts for team

## ?? Documentation Checklist

### Have Read
- [ ] QUICK_START.md - Quick reference
- [ ] DATABASE_SETUP.md - Detailed setup guide
- [ ] SETUP_SUMMARY.md - Files overview
- [ ] ARCHITECTURE.md - System architecture
- [ ] This checklist (CHECKLIST.md)

### Have Available
- [ ] Database schema file: AWE_Electronics_Database.sql
- [ ] All PowerShell scripts for management
- [ ] Connection information saved somewhere safe

## ?? Final Verification

### Everything Works?
- [ ] Docker container is running
- [ ] Database is accessible
- [ ] Application builds successfully
- [ ] Can log into application
- [ ] Sample data is visible
- [ ] No error messages
- [ ] Ready to start development!

## ? Troubleshooting If Something Failed

If any item above failed, consult:

1. **Docker Issues**
   - Check: Docker Desktop is running (green icon)
   - Run: `docker ps` to see if container is up
   - Check: `docker logs sqlserver2022` for errors

2. **Connection Issues**
   - Run: `.\test-database-connection.ps1`
   - Wait: 30-60 seconds after starting container
   - Verify: Port 1433 is not blocked by firewall
   - Check: `netstat -ano | findstr :1433`

3. **Database Issues**
   - Re-run: `.\import-database-schema.ps1`
   - Check: SSMS can connect
   - Verify: Database tables exist

4. **Application Issues**
   - Clean solution: Build ? Clean Solution
   - Rebuild: Build ? Rebuild Solution
   - Check: Connection string in DatabaseConfig.cs
   - Verify: All NuGet packages restored

5. **Need Help?**
   - See: DATABASE_SETUP.md Troubleshooting section
   - Check: Docker logs
   - Verify: All prerequisites installed

## ?? Success Criteria

You're done when:

? All checkboxes above are checked  
? Container is running smoothly  
? Database is accessible  
? Application launches and runs  
? Can log in successfully  
? Sample data is visible  
? No errors in console/logs  

---

**Congratulations! Your AWE Electronics development environment is ready! ??**

**Next Steps:**
1. Start building features
2. Customize the application
3. Add more products and categories
4. Test thoroughly
5. Deploy to production when ready

**Remember:** 
- Keep Docker running while developing
- Back up your database regularly
- Change default passwords before production
- Review security best practices

---

*Last Updated: 2024*  
*AWE Electronics Setup v1.0*
