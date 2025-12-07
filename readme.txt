================================================================================
                    AWE ELECTRONICS MANAGEMENT SYSTEM
                    LIBRARIES, IDE & COMPILATION GUIDE
================================================================================

PROJECT OVERVIEW
================================================================================
AWE Electronics is a comprehensive electronics management system with two
versions: Desktop (Windows Forms) and Web (ASP.NET MVC). Both versions share
common business logic (BLL), data access layer (DAL), and data transfer
objects (DTO).


================================================================================
                          SYSTEM REQUIREMENTS
================================================================================

BOTH VERSIONS REQUIRE:
----------------------
1. Operating System: Windows 10 or later
2. IDE: Visual Studio 2017 or later (Community, Professional, or Enterprise)
   - Download from: https://visualstudio.microsoft.com/downloads/
3. .NET Framework: 4.8 or later
4. Database: SQL Server 2019 or later (Docker recommended)
5. Docker Desktop (for SQL Server container)
   - Download from: https://www.docker.com/products/docker-desktop/
6. Git (optional, for version control)


================================================================================
                        SOLUTION STRUCTURE
================================================================================

AWEElectronics.sln
|
+-- DTO (Data Transfer Objects)
|   +-- Framework: .NET Framework 4.8
|   +-- Purpose: Data models and DTOs
|
+-- DAL (Data Access Layer)
|   +-- Framework: .NET Framework 4.8
|   +-- Database: SQL Server via ADO.NET
|   +-- Libraries:
|       - System.Data
|       - System.Configuration
|
+-- BLL (Business Logic Layer)
|   +-- Framework: .NET Framework 4.8
|   +-- Libraries:
|       - BCrypt.Net-Next 4.0.3 (password hashing)
|
+-- Desktop (Windows Forms Application)
|   +-- Framework: .NET Framework 4.8
|   +-- UI Technology: Windows Forms
|   +-- Libraries: (see Desktop Libraries section)
|
+-- Web (ASP.NET MVC Web Application)
    +-- Framework: .NET Framework 4.8
    +-- UI Technology: ASP.NET MVC 5 with Razor
    +-- Libraries: (see Web Libraries section)


================================================================================
                    DESKTOP VERSION (Windows Forms)
================================================================================

IDE & TOOLS
-----------
- Visual Studio 2017 or later with:
  * .NET desktop development workload
  * Windows Forms components
  * NuGet Package Manager

TARGET FRAMEWORK
----------------
- .NET Framework 4.8

LIBRARIES & DEPENDENCIES
------------------------
NuGet Packages:
  1. BCrypt.Net-Next (4.0.3)
     - Purpose: Secure password hashing
     - License: MIT

  2. System.Buffers (4.5.1)
     - Purpose: Memory management (BCrypt dependency)

  3. System.Memory (4.5.4)
     - Purpose: Memory management (BCrypt dependency)

  4. System.Numerics.Vectors (4.5.0)
     - Purpose: Numeric operations (BCrypt dependency)

  5. System.Runtime.CompilerServices.Unsafe (4.5.3)
     - Purpose: Low-level operations (BCrypt dependency)

System References:
  - System.Windows.Forms
  - System.Drawing
  - System.Data
  - System.Configuration
  - System.Xml
  - System.Net.Http

DATABASE CONNECTION
-------------------
Connection String: Configured in Desktop\App.config
  Server: localhost,1433
  Database: AWEElectronics_DB
  Authentication: SQL Server (sa user)
  TrustServerCertificate: True

FEATURES
--------
- User authentication and authorization
- Product management (CRUD operations)
- Order management and tracking
- Report generation
- Inventory management
- Role-based access control (Admin, Manager, Staff, Accountant, Agent)


================================================================================
                    WEB VERSION (ASP.NET MVC)
================================================================================

IDE & TOOLS
-----------
- Visual Studio 2017 or later with:
  * ASP.NET and web development workload
  * .NET Framework 4.8
  * IIS Express
  * NuGet Package Manager

TARGET FRAMEWORK
----------------
- .NET Framework 4.8
- ASP.NET MVC 5.2.9

WEB SERVER
----------
- IIS Express (Development)
- IIS 8.0 or later (Production)
- HTTPS Port: 44395 (Development)

LIBRARIES & DEPENDENCIES
------------------------
NuGet Packages:

  A. ASP.NET Framework:
     1. Microsoft.AspNet.Mvc (5.2.9)
        - Purpose: MVC framework
     2. Microsoft.AspNet.Razor (3.2.9)
        - Purpose: Razor view engine
     3. Microsoft.AspNet.WebPages (3.2.9)
        - Purpose: Web pages support
     4. Microsoft.AspNet.Web.Optimization (1.1.3)
        - Purpose: Bundling and minification
     5. Microsoft.Web.Infrastructure (2.0.0)
        - Purpose: Web infrastructure support

  B. Front-End Libraries:
     1. Bootstrap (5.2.3)
        - Purpose: Responsive UI framework
        - CSS and JavaScript components
     2. jQuery (3.7.0)
        - Purpose: JavaScript library for DOM manipulation
     3. jQuery.Validation (1.19.5)
        - Purpose: Client-side form validation
     4. Microsoft.jQuery.Unobtrusive.Validation (3.2.11)
        - Purpose: Unobtrusive validation with jQuery
     5. Modernizr (2.8.3)
        - Purpose: HTML5/CSS3 feature detection

  C. Security & Utilities:
     1. BCrypt.Net-Next (4.0.3)
        - Purpose: Password hashing and verification
     2. Newtonsoft.Json (13.0.3)
        - Purpose: JSON serialization/deserialization

  D. Build & Compilation:
     1. Microsoft.CodeDom.Providers.DotNetCompilerPlatform (2.0.1)
        - Purpose: Roslyn compiler for dynamic compilation
     2. WebGrease (1.6.0)
        - Purpose: Asset optimization
     3. Antlr (3.5.0.2)
        - Purpose: Parser generator (WebGrease dependency)

DATABASE CONNECTION
-------------------
Connection String: Configured in Web\Web.config
  Server: localhost,1433
  Database: AWEElectronics_DB
  Authentication: SQL Server (sa user)
  TrustServerCertificate: True

FEATURES
--------
- User authentication with session management
- Role-based dashboards (Admin, Manager, Staff, Accountant, Agent)
- Product catalog and management
- Order processing and tracking
- Customer management
- Shopping cart functionality
- Report generation and viewing
- Responsive design (mobile-friendly)
- AJAX-based operations


================================================================================
                        DATABASE SETUP
================================================================================

DATABASE ENGINE
---------------
- SQL Server 2019 or later
- Recommended: SQL Server in Docker container

DOCKER SETUP (RECOMMENDED)
--------------------------
1. Install Docker Desktop for Windows
2. Open PowerShell as Administrator
3. Run the setup script:

   .\setup-web-version.ps1

   This script will:
   - Create and start SQL Server Docker container
   - Import database schema
   - Create sample data
   - Configure connection strings

MANUAL DATABASE SETUP
---------------------
If not using Docker:

1. Install SQL Server 2019 or later
2. Enable SQL Server Authentication
3. Open SQL Server Management Studio (SSMS)
4. Execute database script:
   - File: Database\AWE_Electronics_WebStore_Schema.sql
5. Update connection strings in:
   - Desktop\App.config
   - Web\Web.config

DATABASE SCHEMA
---------------
The database includes:
- Users (staff authentication)
- Customers (customer accounts)
- Products (product catalog)
- Categories (product categories)
- Orders (order management)
- OrderDetails (order line items)
- Payments (payment tracking)
- Suppliers (supplier management)
- InventoryTransactions (stock tracking)
- Addresses (customer addresses)
- CartItems (shopping cart)

Sample Data:
- 5 staff users (admin, manager, staff, accountant, agent)
- 5 customer accounts
- 16 products
- 12 product categories
- 4 suppliers


================================================================================
            HOW TO COMPILE AND RUN - DESKTOP VERSION
================================================================================

STEP 1: OPEN THE SOLUTION
--------------------------
1. Open Visual Studio 2017 or later
2. File -> Open -> Project/Solution
3. Navigate to: D:\AWEElectronics\
4. Select: AWEElectronics.sln
5. Click "Open"

STEP 2: RESTORE NUGET PACKAGES
-------------------------------
1. Right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for package restoration to complete

STEP 3: SET DESKTOP AS STARTUP PROJECT
---------------------------------------
1. In Solution Explorer, right-click "Desktop" project
2. Select "Set as StartUp Project"

STEP 4: VERIFY DATABASE CONNECTION
-----------------------------------
1. Ensure Docker Desktop is running
2. Verify SQL Server container is running:

   docker ps

   Should show a container named "sqlserver"

3. If not running, start it:

   docker start sqlserver

   OR run the setup script:

   .\setup-web-version.ps1

STEP 5: BUILD THE SOLUTION
---------------------------
1. Build -> Build Solution (or press Ctrl+Shift+B)
2. Check Output window for any errors
3. Ensure all projects build successfully

STEP 6: RUN THE DESKTOP APPLICATION
------------------------------------
1. Debug -> Start Debugging (or press F5)
   OR
   Debug -> Start Without Debugging (or press Ctrl+F5)

2. The login form will appear
3. Login with default credentials:
   - Username: admin
   - Password: admin123

ALTERNATIVE LOGIN ACCOUNTS
---------------------------
Role: Admin
  Username: admin
  Password: admin123

Role: Manager
  Username: manager
  Password: manager123

Role: Staff
  Username: staff
  Password: staff123

Role: Accountant
  Username: accountant
  Password: accountant123

Role: Agent
  Username: agent
  Password: agent123


================================================================================
              HOW TO COMPILE AND RUN - WEB VERSION
================================================================================

STEP 1: OPEN THE SOLUTION
--------------------------
1. Open Visual Studio 2017 or later
2. File -> Open -> Project/Solution
3. Navigate to: D:\AWEElectronics\
4. Select: AWEElectronics.sln
5. Click "Open"

STEP 2: RESTORE NUGET PACKAGES
-------------------------------
1. Right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for package restoration to complete

STEP 3: SET WEB AS STARTUP PROJECT
-----------------------------------
1. In Solution Explorer, right-click "Web" project
2. Select "Set as StartUp Project"

STEP 4: RUN AUTOMATED SETUP (FIRST TIME)
-----------------------------------------
1. Open PowerShell in the solution root directory
2. Run the setup script:

   .\setup-web-version.ps1

3. Wait for "SUCCESS" message
4. The script will:
   - Start SQL Server in Docker
   - Create database and schema
   - Import sample data
   - Configure Web.config

STEP 5: BUILD THE SOLUTION
---------------------------
1. Build -> Build Solution (or press Ctrl+Shift+B)
2. Check Output window for any errors
3. Ensure all projects build successfully

STEP 6: RUN THE WEB APPLICATION
--------------------------------
1. Debug -> Start Debugging (or press F5)
   OR
   Debug -> Start Without Debugging (or press Ctrl+F5)

2. Browser will open automatically at: https://localhost:44395/

3. You will see the login page

4. Login with staff credentials:
   - Username: admin
   - Password: admin123

WEB APPLICATION URL
-------------------
Development: https://localhost:44395/
Login Page: https://localhost:44395/Account/StaffLogin

DEFAULT WEB ACCOUNTS
--------------------
Staff Accounts (Backend Access):

  Admin Account:
    Username: admin
    Password: admin123
    Access: Full system access

  Manager Account:
    Username: manager
    Password: manager123
    Access: Store management

  Staff Account:
    Username: staff
    Password: staff123
    Access: Order processing

  Accountant Account:
    Username: accountant
    Password: accountant123
    Access: Financial reports

  Agent Account:
    Username: agent
    Password: agent123
    Access: Customer support

Customer Accounts (Frontend - Future Implementation):
  Email: alice.nguyen@email.com
  Password: customer123


================================================================================
                        TROUBLESHOOTING
================================================================================

COMMON ISSUES & SOLUTIONS
--------------------------

1. DATABASE CONNECTION FAILS
   Problem: Cannot connect to SQL Server
   Solution:
   - Ensure Docker Desktop is running
   - Check SQL Server container status: docker ps
   - Restart container: docker restart sqlserver
   - Verify connection string in App.config/Web.config
   - Test connection: .\test-database-connection.ps1

2. NUGET PACKAGE RESTORE FAILS
   Problem: Packages not downloading
   Solution:
   - Tools -> Options -> NuGet Package Manager -> Package Sources
   - Ensure nuget.org is enabled
   - Clear NuGet cache: Tools -> NuGet Package Manager -> Package Manager Settings
   - Close and reopen Visual Studio

3. BUILD ERRORS - MISSING REFERENCES
   Problem: Cannot find System.Memory or BCrypt assemblies
   Solution:
   - Clean Solution: Build -> Clean Solution
   - Restore NuGet Packages
   - Rebuild Solution: Build -> Rebuild Solution

4. WEB APPLICATION - PORT ALREADY IN USE
   Problem: Port 44395 is already in use
   Solution:
   - Stop IIS Express from system tray
   - Or change port in Web.csproj:
     <IISExpressSSLPort>44396</IISExpressSSLPort>

5. LOGIN FAILS - INVALID CREDENTIALS
   Problem: Cannot login with default credentials
   Solution:
   - Verify database has sample data
   - Re-run setup script: .\setup-web-version.ps1
   - Check Users table:
     sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" ^
     -d AWEElectronics_DB -Q "SELECT * FROM Users"

6. DESKTOP APP - SYSTEM.MEMORY VERSION CONFLICT
   Problem: Could not load file or assembly 'System.Memory'
   Solution:
   - Binding redirects are configured in App.config
   - Verify all BCrypt dependencies are installed
   - Clean and rebuild solution

7. WEB APP - CSS/JAVASCRIPT NOT LOADING
   Problem: Bootstrap or jQuery not working
   Solution:
   - Build -> Clean Solution
   - Delete bin and obj folders
   - Rebuild solution
   - Clear browser cache (Ctrl+Shift+Delete)

8. DATABASE SCHEMA OUTDATED
   Problem: Tables or columns missing
   Solution:
   - Re-import database schema:
     sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" ^
     -i "Database\AWE_Electronics_WebStore_Schema.sql"


================================================================================
                        DEVELOPMENT TIPS
================================================================================

FOR DESKTOP VERSION:
--------------------
1. Use Visual Studio Designer for form layouts
2. Set form properties in Designer, not code
3. Test with different user roles
4. Use try-catch blocks for database operations
5. Log errors to help with debugging

FOR WEB VERSION:
----------------
1. Use Bootstrap classes for responsive design
2. Leverage Razor syntax for server-side rendering
3. Implement AJAX for dynamic updates
4. Test in multiple browsers (Chrome, Edge, Firefox)
5. Use Browser Link for live reload during development
6. Enable detailed error messages in development:
   <customErrors mode="Off" /> in Web.config

DEBUGGING:
----------
1. Set breakpoints (F9) in code
2. Use Watch windows to inspect variables
3. Check Output window for build messages
4. Review Error List for compilation errors
5. Use SQL Server Profiler to debug database queries


================================================================================
                    DEPLOYMENT CONSIDERATIONS
================================================================================

DESKTOP VERSION DEPLOYMENT:
---------------------------
1. Build in Release mode (not Debug)
2. Use ClickOnce or create installer with:
   - Setup project (Visual Studio Installer Projects)
   - Or third-party tools (InstallShield, WiX)
3. Include .NET Framework 4.8 redistributable
4. Configure database connection for production
5. Consider code signing for trusted installation

WEB VERSION DEPLOYMENT:
-----------------------
1. Publish to IIS:
   - Right-click Web project -> Publish
   - Choose IIS, FTP, or Azure
   - Configure Web.config transformations

2. Production Checklist:
   - Change compilation debug="false" in Web.config
   - Use production connection string
   - Enable custom errors: <customErrors mode="On" />
   - Configure SSL certificate
   - Set up application pool in IIS
   - Enable authentication and authorization
   - Configure session timeout
   - Set up backup and monitoring

3. See detailed deployment guide:
   - Deployment\WEB_DEPLOYMENT_GUIDE.md


================================================================================
                        ADDITIONAL RESOURCES
================================================================================

DOCUMENTATION FILES:
--------------------
- README.md - Project overview
- README_WEB_VERSION.md - Web version guide
- WEB_VERSION_QUICK_START.md - Quick setup for web
- WEB_VERSION_VISUAL_GUIDE.md - Visual architecture guide
- HOW_TO_RUN.md - Detailed running instructions
- DATABASE_SETUP.md - Database configuration guide
- ARCHITECTURE.md - System architecture documentation

POWERSHELL SCRIPTS:
-------------------
- setup-web-version.ps1 - Automated web setup
- setup-sqlserver.ps1 - SQL Server setup
- test-database-connection.ps1 - Connection testing
- import-database.ps1 - Database import
- verify-dashboards.ps1 - Dashboard verification
- complete-setup.ps1 - Complete system setup

DATABASE FILES:
---------------
- Database\AWE_Electronics_WebStore_Schema.sql - Complete database schema
- AWE_Electronics_Database.sql - Original database script
- populate-sample-data.sql - Sample data script


================================================================================
                        CONTACT & SUPPORT
================================================================================

For issues or questions:
1. Check documentation files in the project root
2. Review troubleshooting section above
3. Check error logs in:
   - Desktop: Application output
   - Web: Browser console and server logs
4. Verify all prerequisites are installed
5. Ensure database is running and accessible


================================================================================
                            VERSION INFO
================================================================================

Project: AWE Electronics Management System
Version: 1.0
Framework: .NET Framework 4.8
Database: SQL Server 2019+
License: Educational/Commercial Use

Last Updated: December 2025

================================================================================
                            END OF README
================================================================================
