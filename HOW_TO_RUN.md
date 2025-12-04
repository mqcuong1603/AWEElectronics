# ?? How to Run AWE Electronics Project

## Project Overview

Your AWE Electronics solution contains:
- **Desktop** - Windows Forms/WPF Desktop Application (Main UI)
- **Web** - Web Application/API
- **BLL** - Business Logic Layer
- **DAL** - Data Access Layer
- **DTO** - Data Transfer Objects

---

## ? Prerequisites Checklist

Before running the project, ensure you have:

- [x] ? **SQL Server** - Running in Docker (already set up!)
- [x] ? **Database** - AWEElectronics_DB (already imported!)
- [ ] **Visual Studio 2022** (or 2019)
- [ ] **.NET Framework 4.8.1** (comes with Visual Studio)

---

## ?? Quick Start - Run Desktop Application

### Option 1: Using Visual Studio (Recommended)

1. **Open the Solution**
   ```
   Double-click: D:\SE\final\AWEElectronics\AWEElectronics.sln
   ```

2. **Set Startup Project**
   - In Solution Explorer, right-click **Desktop** project
   - Select **"Set as Startup Project"**
   - (The Desktop project should appear **bold**)

3. **Verify Connection String**
   - Open `DAL\DatabaseConfig.cs` or check `App.config`
   - Ensure connection string is:
   ```
   Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;
   ```

4. **Build the Solution**
   - Press **Ctrl + Shift + B** or
   - Menu: **Build ? Build Solution**
   - Wait for "Build succeeded" message

5. **Run the Application**
   - Press **F5** (Debug mode) or **Ctrl + F5** (Without debugging)
   - The AWE Electronics desktop app should launch!

6. **Login with Test Account**
   ```
   Username: admin
   Password: admin123
   ```

---

### Option 2: Using Command Line

```powershell
# Navigate to solution directory
cd D:\SE\final\AWEElectronics

# Restore NuGet packages
nuget restore AWEElectronics.sln

# Build the solution
msbuild AWEElectronics.sln /p:Configuration=Release

# Run the Desktop application
.\Desktop\bin\Release\Desktop.exe
```

---

## ?? Run Web Application (If Available)

### Option 1: Visual Studio

1. **Set Web as Startup Project**
   - Right-click **Web** project
   - Select **"Set as Startup Project"**

2. **Run with IIS Express**
   - Press **F5**
   - Browser should open automatically

### Option 2: Command Line

```powershell
cd D:\SE\final\AWEElectronics\Web
dotnet run
# Or for older .NET Framework:
iisexpress /path:"D:\SE\final\AWEElectronics\Web"
```

---

## ?? Common Issues & Solutions

### Issue 1: "Cannot connect to database"

**Solution:**
```powershell
# 1. Verify SQL Server is running
docker ps

# 2. If not running, start it
docker start sqlserver2022

# 3. Test connection
.\test-connection.ps1
```

### Issue 2: "Build failed - missing references"

**Solution:**
1. Right-click solution ? **"Restore NuGet Packages"**
2. Clean solution: **Build ? Clean Solution**
3. Rebuild: **Build ? Rebuild Solution**

### Issue 3: "Login failed"

**Solution:**
- Verify database connection in `DAL\DatabaseConfig.cs`
- Use test credentials:
  - Username: `admin` Password: `admin123`
  - Username: `manager` Password: `manager123`
  - Username: `staff` Password: `staff123`

### Issue 4: "Missing .NET Framework 4.8.1"

**Solution:**
1. Download from: https://dotnet.microsoft.com/download/dotnet-framework
2. Or update in Visual Studio Installer

### Issue 5: "Port already in use" (Web app)

**Solution:**
- Check `Web\Properties\launchSettings.json` or `Web.config`
- Change the port number
- Or stop other applications using the same port

---

## ?? Project Structure Quick Reference

```
AWEElectronics/
?
??? Desktop/              ? Main UI Application (Run This!)
?   ??? Forms/           - User interface forms
?   ??? App.config       - Configuration file
?   ??? Program.cs       - Entry point
?
??? Web/                 - Web application (Optional)
?   ??? Web.config       - Web configuration
?
??? BLL/                 - Business logic
?   ??? Services/        - Business services
?
??? DAL/                 - Database access
?   ??? DatabaseConfig.cs - Connection string
?   ??? Repositories/    - Data access classes
?
??? DTO/                 - Data models
?   ??? Models/          - Entity classes
?
??? AWE_Electronics_Database.sql - Database schema
```

---

## ?? Testing the Application

### 1. Test Login
```
Username: admin
Password: admin123
Role: Admin (full access)
```

### 2. Test Features
- **Products**: Browse 14 sample products
- **Categories**: View 11 product categories
- **Suppliers**: Manage 3 suppliers
- **Inventory**: Check stock levels
- **Orders**: Create test orders
- **Users**: Manage user accounts (admin only)

### 3. Test Database
```sql
-- In SSMS, run this query:
SELECT COUNT(*) AS TotalProducts FROM Products;
-- Should return: 14
```

---

## ?? Development Workflow

### Daily Workflow
1. **Start Docker** (if not running)
   ```powershell
   docker start sqlserver2022
   ```

2. **Open Visual Studio**
   - Open `AWEElectronics.sln`

3. **Set Desktop as Startup**
   - Right-click Desktop ? Set as Startup Project

4. **Press F5 to Run**

5. **Make changes, test, repeat**

### Before Committing Code
```powershell
# 1. Build in Release mode
msbuild AWEElectronics.sln /p:Configuration=Release

# 2. Run tests (if you have test project)
dotnet test

# 3. Check for errors
# Review build output
```

---

## ?? Performance Tips

### Faster Builds
- Build only changed projects
- Use **Ctrl + F5** (run without debugging) for faster startup
- Enable parallel builds: Tools ? Options ? Projects and Solutions ? Build and Run

### Debugging Tips
- Set breakpoints: Click left margin in code editor
- Watch variables: Right-click variable ? Add Watch
- Use Immediate Window: Debug ? Windows ? Immediate

---

## ?? Next Steps After Running

1. **Explore the UI**
   - Login as different users (admin/manager/staff)
   - Test different features
   - Check how permissions work

2. **Review the Code**
   - Start with `Desktop\Program.cs` (entry point)
   - Check `DAL\DatabaseConfig.cs` (database connection)
   - Review `BLL` (business logic)

3. **Make Changes**
   - Add new features
   - Customize the UI
   - Add new reports

4. **Test Thoroughly**
   - Test with different user roles
   - Test error handling
   - Test edge cases

---

## ?? Quick Commands Reference

```powershell
# Start SQL Server
docker start sqlserver2022

# Check SQL Server status
docker ps

# View logs
docker logs sqlserver2022

# Test database connection
.\test-connection.ps1

# Build solution
msbuild AWEElectronics.sln

# Open in Visual Studio
start AWEElectronics.sln

# Clean build artifacts
msbuild AWEElectronics.sln /t:Clean
```

---

## ?? Need Help?

### Check These First:
1. ? Docker is running (`docker ps`)
2. ? Database exists (`.\test-connection.ps1`)
3. ? Visual Studio is updated
4. ? Solution builds without errors

### Documentation:
- Database Setup: `DATABASE_SETUP.md`
- Quick Reference: `SETUP_COMPLETE.md`

---

## ?? You're Ready!

**To run the application right now:**

1. Open Visual Studio
2. Open `AWEElectronics.sln`
3. Right-click **Desktop** ? Set as Startup Project
4. Press **F5**
5. Login with `admin` / `admin123`

**Enjoy building with AWE Electronics! ??**

---

**Last Updated:** 2025-12-04  
**Status:** ? Ready to Run
