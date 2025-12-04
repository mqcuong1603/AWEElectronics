# AWE Electronics - Inventory Management System

Complete desktop and web application for electronics inventory management.

---

## ?? **QUICK START - RUN THE PROJECT**

### Method 1: Visual Studio (Easiest)

1. **Open Solution**
   ```
   Double-click: AWEElectronics.sln
   ```

2. **Set Startup Project**
   - Right-click `Desktop` project ? **Set as Startup Project**

3. **Run Application**
   - Press **F5** (or click ? Start button)

4. **Login**
   ```
   Username: admin
   Password: admin123
   ```

### Method 2: Quick Start Script

```powershell
.\START.ps1
```
(Automatically checks prerequisites and opens Visual Studio)

---

## ?? **Prerequisites**

? **Already Set Up:**
- [x] SQL Server 2022 (Docker) - Running on `localhost,1433`
- [x] Database `AWEElectronics_DB` - Imported with sample data
- [x] 3 test user accounts (admin, manager, staff)
- [x] 14 sample products, 11 categories, 3 suppliers

?? **You Need:**
- [ ] Visual Studio 2022 or 2019
- [ ] .NET Framework 4.8.1

---

## ?? **Project Structure**

```
AWEElectronics/
??? Desktop/          ? Desktop Application (Main UI) - START HERE
??? Web/              ? Web Application/API
??? BLL/              ? Business Logic Layer
??? DAL/              ? Data Access Layer (DB Connection)
??? DTO/              ? Data Transfer Objects
??? Database Files    ? SQL scripts and setup
```

---

## ?? **Test User Accounts**

| Username | Password | Role | Access Level |
|----------|----------|------|--------------|
| `admin` | `admin123` | Admin | Full system access |
| `manager` | `manager123` | Manager | Management functions |
| `staff` | `staff123` | Staff | Basic operations |

---

## ?? **Database Connection**

Already configured in `DAL\DatabaseConfig.cs`:

```
Server=localhost,1433
Database=AWEElectronics_DB
User Id=sa
Password=YourStrong@Password123
```

**Manage Docker Container:**
```powershell
docker start sqlserver2022    # Start
docker stop sqlserver2022     # Stop
docker ps                     # Check status
```

---

## ?? **Sample Data Included**

- **Users:** 3 accounts (admin, manager, staff)
- **Products:** 14 electronics items (CPUs, GPUs, RAM, etc.)
- **Categories:** 11 product categories
- **Suppliers:** 3 vendors
- **Ready-to-use:** Create orders, manage inventory

---

## ??? **Common Commands**

```powershell
# Build solution
msbuild AWEElectronics.sln

# Clean build
msbuild AWEElectronics.sln /t:Clean

# Rebuild all
msbuild AWEElectronics.sln /t:Rebuild

# Run Desktop app (after build)
.\Desktop\bin\Debug\Desktop.exe

# Test database connection
.\test-connection.ps1
```

---

## ?? **Documentation**

| File | Description |
|------|-------------|
| `HOW_TO_RUN.md` | **Complete guide** to run the project |
| `DATABASE_SETUP.md` | Database setup and management |
| `SETUP_COMPLETE.md` | Quick reference card |
| `START.ps1` | Automated startup script |

---

## ?? **Features**

### Desktop Application
- ? User authentication and authorization
- ? Product catalog management
- ? Category and supplier management
- ? Inventory tracking
- ? Order processing
- ? Payment management
- ? User management (admin only)
- ? Stock level monitoring
- ? Sales reporting

### Web Application (if configured)
- ? RESTful API
- ? Web-based management
- ? Mobile-responsive design

---

## ?? **Troubleshooting**

### "Cannot connect to database"
```powershell
docker start sqlserver2022
.\test-connection.ps1
```

### "Build failed"
- Right-click solution ? Restore NuGet Packages
- Build ? Clean Solution
- Build ? Rebuild Solution

### "Login failed"
- Use test credentials above
- Check database is running: `docker ps`

### More help
See `HOW_TO_RUN.md` for detailed troubleshooting guide.

---

## ?? **Next Steps**

1. ? Open `AWEElectronics.sln` in Visual Studio
2. ? Set `Desktop` as startup project
3. ? Press F5 to run
4. ? Login with `admin` / `admin123`
5. ? Explore the application!

---

## ?? **Quick Tips**

- **F5** = Run with debugging
- **Ctrl + F5** = Run without debugging (faster)
- **Ctrl + Shift + B** = Build solution
- **Solution Explorer** ? Right-click Desktop ? **Set as Startup Project**

---

## ?? **Security Note**

?? **Development credentials only!** Change passwords before production deployment.

---

## ?? **Need Help?**

1. Check `HOW_TO_RUN.md` for detailed instructions
2. Run `.\test-connection.ps1` to verify database
3. Check Docker is running: `docker ps`
4. Ensure Visual Studio is installed

---

**Status:** ? Ready to Run  
**Last Updated:** 2025-12-04  
**Database:** ? Running & Populated

---

## ?? **You're All Set!**

Just double-click `AWEElectronics.sln` and press F5!

Happy coding! ??
