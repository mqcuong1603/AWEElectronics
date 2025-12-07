================================================================================
                    AWE ELECTRONICS MANAGEMENT SYSTEM
                          INSTALLATION GUIDE
================================================================================

PROJECT OVERVIEW
================================================================================
This solution contains two versions of the AWE Electronics Management System:
1. Desktop Application (Windows Forms)
2. Web Application (ASP.NET MVC)

Both applications share the same business logic (BLL), data access layer (DAL),
and data transfer objects (DTO).


SYSTEM REQUIREMENTS
================================================================================

1. OPERATING SYSTEM
   - Windows 10 or later (for Desktop)
   - Windows Server 2016 or later (for Web deployment)

2. DEVELOPMENT TOOLS
   - Visual Studio 2017 or later (Recommended: Visual Studio 2022)
   - .NET Framework 4.8 SDK
   - NuGet Package Manager (included with Visual Studio)

3. DATABASE
   - SQL Server 2016 or later
   OR
   - Docker Desktop (for running SQL Server in container)

4. WEB SERVER (for Web version only)
   - IIS 10.0 or later
   OR
   - IIS Express (included with Visual Studio)


PROJECT STRUCTURE
================================================================================

AWEElectronics/
├── AWEElectronics.sln          - Main solution file
├── DTO/                        - Data Transfer Objects
│   └── DTO.csproj
├── DAL/                        - Data Access Layer
│   ├── DAL.csproj
│   └── DatabaseConfig.cs       - Database connection configuration
├── BLL/                        - Business Logic Layer
│   └── BLL.csproj
├── Desktop/                    - Windows Forms Desktop Application
│   ├── Desktop.csproj
│   └── Forms/                  - UI Forms
└── Web/                        - ASP.NET MVC Web Application
    ├── Web.csproj
    ├── Controllers/            - MVC Controllers
    ├── Views/                  - Razor Views
    └── Content/                - CSS, Images, Scripts


LIBRARIES AND DEPENDENCIES
================================================================================

CORE LIBRARIES (Used by all projects):
--------------------------------------
- BCrypt.Net-Next 4.0.3         - Password hashing and security
- System.Buffers 4.5.1          - Memory management
- System.Memory 4.5.5           - Memory management
- System.Numerics.Vectors 4.5.0 - Performance optimization
- System.Runtime.CompilerServices.Unsafe 6.0.0

WEB APPLICATION ADDITIONAL LIBRARIES:
--------------------------------------
- ASP.NET MVC 5.2.9             - Web framework
- ASP.NET Razor 3.2.9           - View engine
- ASP.NET Web Pages 3.2.9       - Web pages framework
- Bootstrap 5.2.3               - CSS framework
- jQuery 3.7.0                  - JavaScript library
- jQuery Validation 1.19.5      - Client-side validation
- Newtonsoft.Json 13.0.3        - JSON serialization
- Modernizr 2.8.3               - Browser feature detection
- WebGrease 1.6.0               - Asset optimization
- Microsoft.Web.Infrastructure 2.0.0
- Microsoft.AspNet.Web.Optimization 1.1.3

All dependencies are managed through NuGet and will be automatically restored
when you build the solution.


DATABASE SETUP
================================================================================

OPTION 1: Using Docker (Recommended for Development)
-----------------------------------------------------
1. Install Docker Desktop from https://www.docker.com/products/docker-desktop

2. Open Command Prompt or PowerShell and run:

   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" ^
   -p 1433:1433 --name sql_server_awe ^
   -d mcr.microsoft.com/mssql/server:2019-latest

3. Verify the container is running:
   docker ps

4. The database connection is already configured in DAL/DatabaseConfig.cs:
   - Server: localhost,1433
   - Database: AWEElectronics_DB
   - User: sa
   - Password: YourStrong@Password123


OPTION 2: Using Local SQL Server Installation
----------------------------------------------
1. Install SQL Server 2016 or later from Microsoft's website

2. During installation, choose "Mixed Mode Authentication" and set SA password

3. After installation, update the connection string in:
   DAL/DatabaseConfig.cs

   Example:
   public static string ConnectionString =
       "Server=localhost;" +
       "Database=AWEElectronics_DB;" +
       "User Id=sa;" +
       "Password=YOUR_PASSWORD;" +
       "TrustServerCertificate=True;";

4. Create the database:
   - Open SQL Server Management Studio (SSMS)
   - Connect to your SQL Server instance
   - Create a new database named: AWEElectronics_DB
   - Run any initialization scripts (if provided)


BUILDING AND RUNNING - DESKTOP VERSION
================================================================================

USING VISUAL STUDIO:
--------------------
1. Open AWEElectronics.sln in Visual Studio

2. Restore NuGet Packages:
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"
   - Wait for all packages to download

3. Set Desktop as Startup Project:
   - Right-click on "Desktop" project in Solution Explorer
   - Select "Set as StartUp Project"

4. Build the solution:
   - Press Ctrl+Shift+B
   OR
   - Menu: Build > Build Solution

5. Run the Desktop Application:
   - Press F5 (Debug mode)
   OR
   - Press Ctrl+F5 (Run without debugging)
   OR
   - Menu: Debug > Start Debugging

6. The Desktop application executable will be created at:
   Desktop/bin/Debug/Desktop.exe (Debug build)
   Desktop/bin/Release/Desktop.exe (Release build)


USING COMMAND LINE (MSBuild):
------------------------------
1. Open "Developer Command Prompt for Visual Studio"

2. Navigate to the project directory:
   cd D:\AWEElectronics

3. Restore NuGet packages:
   nuget restore AWEElectronics.sln

4. Build the Desktop application:
   msbuild Desktop/Desktop.csproj /p:Configuration=Release

5. Run the executable:
   Desktop\bin\Release\Desktop.exe


BUILDING AND RUNNING - WEB VERSION
================================================================================

USING VISUAL STUDIO WITH IIS EXPRESS (Recommended for Development):
-------------------------------------------------------------------
1. Open AWEElectronics.sln in Visual Studio

2. Restore NuGet Packages:
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"

3. Set Web as Startup Project:
   - Right-click on "Web" project in Solution Explorer
   - Select "Set as StartUp Project"

4. Build the solution:
   - Press Ctrl+Shift+B
   OR
   - Menu: Build > Build Solution

5. Run the Web Application:
   - Press F5 (Debug mode)
   OR
   - Press Ctrl+F5 (Run without debugging)

6. The application will open in your default browser at:
   https://localhost:44395/

7. IIS Express will automatically start and host the application


USING COMMAND LINE (MSBuild):
------------------------------
1. Open "Developer Command Prompt for Visual Studio"

2. Navigate to the project directory:
   cd D:\AWEElectronics

3. Restore NuGet packages:
   nuget restore AWEElectronics.sln

4. Build the Web application:
   msbuild Web/Web.csproj /p:Configuration=Release /p:DeployOnBuild=true


DEPLOYING TO IIS (Production):
-------------------------------
1. Install IIS on Windows Server:
   - Open Server Manager
   - Add Roles and Features > Web Server (IIS)
   - Include ASP.NET 4.8 features

2. Build the Web project in Release mode

3. Publish the application:
   - Right-click Web project in Visual Studio
   - Select "Publish"
   - Choose "Folder" as publish target
   - Select output folder (e.g., C:\Publish\AWEElectronics)
   - Click "Publish"

4. Configure IIS:
   - Open IIS Manager
   - Right-click "Sites" > "Add Website"
   - Site name: AWEElectronics
   - Physical path: [Your publish folder]
   - Port: 80 (or desired port)
   - Click OK

5. Configure Application Pool:
   - Select Application Pools
   - Find AWEElectronics pool
   - Set ".NET CLR Version" to "v4.0"
   - Set "Managed Pipeline Mode" to "Integrated"

6. Update Web.config connection string if needed

7. Browse to: http://localhost (or your server address)


CONFIGURATION
================================================================================

DATABASE CONNECTION:
--------------------
To change database connection settings, edit:
DAL/DatabaseConfig.cs

Update the ConnectionString property with your database details.

WEB APPLICATION SETTINGS:
-------------------------
Edit Web/Web.config for:
- Forms authentication timeout (default: 2880 minutes)
- Session settings
- Custom application settings

DESKTOP APPLICATION SETTINGS:
------------------------------
Edit Desktop/App.config for:
- Runtime assembly bindings
- Custom application settings


TROUBLESHOOTING
================================================================================

COMMON ISSUES AND SOLUTIONS:

1. "Could not load file or assembly 'BCrypt.Net-Next'"
   Solution:
   - Right-click solution > Restore NuGet Packages
   - Rebuild the solution

2. "Cannot connect to SQL Server"
   Solution:
   - Verify SQL Server is running (docker ps or SQL Server Services)
   - Check connection string in DAL/DatabaseConfig.cs
   - Test connection using SQL Server Management Studio
   - Check firewall settings (port 1433)

3. "The type or namespace name 'DTO' could not be found"
   Solution:
   - Ensure all projects are added to the solution
   - Check project references are correct
   - Clean and rebuild solution (Build > Clean Solution, then Build > Rebuild)

4. Web application: "HTTP Error 500.19 - Internal Server Error"
   Solution:
   - Install .NET Framework 4.8 on the server
   - Install ASP.NET 4.8 in IIS
   - Check Web.config for syntax errors

5. Desktop application: "System.BadImageFormatException"
   Solution:
   - Ensure project is set to "Any CPU" or matches your system
   - Check all referenced DLLs are compatible with your platform

6. "Login failed for user 'sa'"
   Solution:
   - Verify SQL Server password in DatabaseConfig.cs
   - Check SQL Server authentication mode (should be Mixed Mode)
   - Verify user permissions on database


PROJECT BUILD ORDER
================================================================================
The projects must be built in this order (Visual Studio handles this automatically):
1. DTO (Data Transfer Objects)
2. DAL (Data Access Layer) - depends on DTO
3. BLL (Business Logic Layer) - depends on DTO and DAL
4. Desktop or Web - depends on DTO, DAL, and BLL


FIRST TIME SETUP CHECKLIST
================================================================================
[ ] Install Visual Studio 2017 or later
[ ] Install .NET Framework 4.8 SDK
[ ] Install SQL Server or Docker Desktop
[ ] Set up SQL Server container or local instance
[ ] Create AWEElectronics_DB database
[ ] Clone/download the source code
[ ] Open AWEElectronics.sln in Visual Studio
[ ] Restore NuGet packages
[ ] Update database connection string (if needed)
[ ] Build the solution
[ ] Run the Desktop or Web application


DEFAULT CREDENTIALS
================================================================================
(Configure these based on your database setup and initial data)

The application uses BCrypt for secure password hashing.
Initial user accounts should be created through the admin interface
or directly in the database.


SUPPORT AND DOCUMENTATION
================================================================================

For questions or issues:
1. Check the Troubleshooting section above
2. Review the code comments in each project
3. Check SQL Server logs for database issues
4. Review Visual Studio build output for compilation errors

Project Architecture:
- 3-tier architecture (DAL, BLL, Presentation)
- Desktop: Windows Forms with .NET Framework 4.8
- Web: ASP.NET MVC 5 with Razor views
- Database: Microsoft SQL Server


VERSION INFORMATION
================================================================================
.NET Framework Version: 4.8
Visual Studio Solution Format: 12.00 (Visual Studio 2017+)
ASP.NET MVC Version: 5.2.9
BCrypt Version: 4.0.3
Bootstrap Version: 5.2.3
jQuery Version: 3.7.0


ADDITIONAL NOTES
================================================================================

1. Security:
   - All passwords are hashed using BCrypt
   - Web application uses Forms Authentication
   - Connection strings contain sensitive data - keep secure

2. Development:
   - Use Debug configuration for development
   - Use Release configuration for production deployment
   - Test both Desktop and Web versions after changes to BLL or DAL

3. Database:
   - Database schema is managed through DAL layer
   - Always backup database before schema changes
   - Use transactions for critical operations

4. Updates:
   - Keep NuGet packages updated for security patches
   - Test thoroughly after updating dependencies
   - Review release notes for breaking changes


================================================================================
                           END OF README
================================================================================
