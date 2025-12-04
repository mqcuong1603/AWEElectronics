# AWE Electronics - Complete Web Deployment Guide

## ?? Table of Contents
1. [Overview](#overview)
2. [Database Deployment Options](#database-deployment-options)
3. [Web Application Deployment](#web-application-deployment)
4. [Configuration for Production](#configuration-for-production)
5. [Docker Deployment](#docker-deployment)
6. [Azure Cloud Deployment](#azure-cloud-deployment)
7. [Security Best Practices](#security-best-practices)

---

## ?? Overview

**YES, you can run SQL Server and AWE Electronics project on the web!**

Your project structure:
- ? **Web Project**: ASP.NET MVC 5 web application (in `Web` folder)
- ? **Database**: SQL Server (can be hosted locally, Docker, or cloud)
- ? **Architecture**: 3-tier (DAL ? BLL ? Web/Desktop)

---

## ?? Database Deployment Options

### **Option 1: Use the Web-Optimized Schema** (Recommended)

The new schema (`Database/AWE_Electronics_WebStore_Schema.sql`) includes:
- ? Customer accounts & authentication
- ? Shopping cart functionality
- ? Multiple shipping addresses
- ? Payment processing
- ? Complete e-commerce features

**Features Added:**
```
- Customers table (user accounts)
- Addresses table (shipping/billing)
- CartItems table (shopping cart)
- Enhanced Orders & Payments
- Stored procedures for cart operations
- Views for product catalog
```

###  **Option 2: Cloud Database - Azure SQL**

**Advantages:**
- No server management
- Automatic backups
- Built-in scaling
- High availability

**Setup:**
```powershell
# Install Azure CLI
winget install Microsoft.AzureCLI

# Login to Azure
az login

# Create resource group
az group create --name AWEElectronics-RG --location eastus

# Create SQL Server
az sql server create `
  --name awe-electronics-server `
  --resource-group AWEElectronics-RG `
  --location eastus `
  --admin-user sqladmin `
  --admin-password "YourStrong@Password123"

# Create database
az sql db create `
  --name AWEElectronics_DB `
  --server awe-electronics-server `
  --resource-group AWEElectronics-RG `
  --service-objective S0

# Configure firewall (allow Azure services)
az sql server firewall-rule create `
  --server awe-electronics-server `
  --resource-group AWEElectronics-RG `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0

# Add your IP
$myIP = (Invoke-WebRequest -Uri "https://api.ipify.org").Content
az sql server firewall-rule create `
  --server awe-electronics-server `
  --resource-group AWEElectronics-RG `
  --name AllowMyIP `
  --start-ip-address $myIP `
  --end-ip-address $myIP
```

**Connection String for Azure SQL:**
```
Server=awe-electronics-server.database.windows.net;
Database=AWEElectronics_DB;
User Id=sqladmin;
Password=YourStrong@Password123;
Encrypt=True;
```

### **Option 3: Docker SQL Server** (Local/VPS)

**Already configured!** Use your existing Docker setup:
```powershell
# Start SQL Server container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" `
  -p 1433:1433 --name sqlserver `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Import schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" `
  -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

---

## ?? Web Application Deployment

### **Step 1: Update Connection String**

Currently hardcoded in `DAL/DatabaseConfig.cs`:
```csharp
public static string ConnectionString =
    "Server=localhost,1433;" +
    "Database=AWEElectronics_DB;" +
    "User Id=sa;" +
    "Password=YourStrong@Password123;" +
    "TrustServerCertificate=True;";
```

**?? MUST CHANGE for Production!**

Create `Web/Web.config` (main config):
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="AWEElectronics" 
         connectionString="Server=your-server;Database=AWEElectronics_DB;User Id=sa;Password=YourPassword;Encrypt=True;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>

  <appSettings>
    <add key="webpages:Version" value="3.0.0.0" />
    <add key="webpages:Enabled" value="false" />
    <add key="ClientValidationEnabled" value="true" />
    <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  </appSettings>

  <system.web>
    <authentication mode="Forms">
      <forms loginUrl="~/Account/Login" timeout="2880" />
    </authentication>
    <compilation debug="false" targetFramework="4.8.1" />
    <httpRuntime targetFramework="4.8.1" />
    <customErrors mode="On" defaultRedirect="~/Error" />
  </system.web>

  <system.webServer>
    <modules runAllManagedModulesForAllRequests="true" />
    <handlers>
      <remove name="ExtensionlessUrlHandler-Integrated-4.0" />
      <add name="ExtensionlessUrlHandler-Integrated-4.0" 
           path="*." verb="*" type="System.Web.Handlers.TransferRequestHandler" 
           preCondition="integratedMode,runtimeVersionv4.0" />
    </handlers>
  </system.webServer>
</configuration>
```

**Update `DAL/DatabaseConfig.cs`:**
```csharp
using System.Configuration;

namespace AWEElectronics.DAL
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["AWEElectronics"]?.ConnectionString
                    ?? "Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;";
            }
        }
    }
}
```

### **Step 2: Deploy to IIS (Windows Server)**

**Prerequisites:**
- Windows Server 2016+ or Windows 10/11 Pro
- IIS with ASP.NET 4.8
- .NET Framework 4.8.1

**Installation:**
```powershell
# Enable IIS
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45

# Or via Server Manager
Install-WindowsFeature -Name Web-Server -IncludeManagementTools
Install-WindowsFeature -Name Web-Asp-Net45
```

**Publish from Visual Studio:**
1. Right-click `Web` project ? **Publish**
2. Choose **Folder** ? `C:\inetpub\wwwroot\AWEElectronics`
3. Configuration: **Release**
4. Click **Publish**

**Configure IIS:**
```powershell
# Create application pool
New-WebAppPool -Name "AWEElectronics"
Set-ItemProperty IIS:\AppPools\AWEElectronics -Name "managedRuntimeVersion" -Value "v4.0"

# Create website
New-Website -Name "AWEElectronics" `
  -Port 80 `
  -PhysicalPath "C:\inetpub\wwwroot\AWEElectronics" `
  -ApplicationPool "AWEElectronics"

# Configure HTTPS (recommended)
New-WebBinding -Name "AWEElectronics" -IP "*" -Port 443 -Protocol https
```

### **Step 3: Deploy to Azure App Service**

**Using Azure CLI:**
```powershell
# Create App Service Plan
az appservice plan create `
  --name AWEElectronics-Plan `
  --resource-group AWEElectronics-RG `
  --sku B1 `
  --is-linux false

# Create Web App
az webapp create `
  --name awe-electronics-web `
  --resource-group AWEElectronics-RG `
  --plan AWEElectronics-Plan `
  --runtime "ASPNET|V4.8"

# Configure connection string
az webapp config connection-string set `
  --name awe-electronics-web `
  --resource-group AWEElectronics-RG `
  --settings AWEElectronics="Server=awe-electronics-server.database.windows.net;Database=AWEElectronics_DB;User Id=sqladmin;Password=YourStrong@Password123;Encrypt=True;" `
  --connection-string-type SQLAzure

# Deploy (from Visual Studio or CLI)
# Via Visual Studio: Right-click project ? Publish ? Azure ? Azure App Service
```

**Via Visual Studio Publish:**
1. Right-click `Web` project ? **Publish**
2. Target: **Azure** ? **Azure App Service (Windows)**
3. Select subscription & create/select app service
4. Configure connection string in Azure Portal
5. Click **Publish**

---

## ?? Docker Deployment (Full Stack)

### **Docker Compose Setup**

Create `docker-compose.yml` in solution root:
```yaml
version: '3.8'

services:
  # SQL Server Database
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: awe-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Password123
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql
      - ./Database:/docker-entrypoint-initdb.d
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong@Password123" -Q "SELECT 1"
      interval: 10s
      timeout: 5s
      retries: 5

  # Web Application (requires Dockerfile)
  webapp:
    build:
      context: .
      dockerfile: Web/Dockerfile
    container_name: awe-webapp
    ports:
      - "80:80"
    depends_on:
      sqlserver:
        condition: service_healthy
    environment:
      - ConnectionStrings__AWEElectronics=Server=sqlserver;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;

volumes:
  sqldata:
    driver: local
```

### **Create Web Dockerfile**

Create `Web/Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

WORKDIR /inetpub/wwwroot

# Copy published output
COPY ./bin/Release/Publish/ ./

EXPOSE 80

# IIS is already configured in base image
```

**Build and Run:**
```powershell
# Build
docker-compose build

# Start services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop
docker-compose down
```

**Initialize Database:**
```powershell
# Wait for SQL Server to be ready
Start-Sleep -Seconds 30

# Run schema script
docker exec awe-sqlserver /opt/mssql-tools/bin/sqlcmd `
  -S localhost -U sa -P "YourStrong@Password123" `
  -i /docker-entrypoint-initdb.d/AWE_Electronics_WebStore_Schema.sql
```

---

## ?? Security Best Practices

### **1. Never Hardcode Connection Strings**
? Use Web.config
? Use Azure Key Vault for production
? Use environment variables in Docker

### **2. Enable HTTPS**
```xml
<!-- Web.config -->
<system.webServer>
  <rewrite>
    <rules>
      <rule name="HTTP to HTTPS" stopProcessing="true">
        <match url="(.*)" />
        <conditions>
          <add input="{HTTPS}" pattern="^OFF$" />
        </conditions>
        <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
      </rule>
    </rules>
  </rewrite>
</system.webServer>
```

### **3. Secure Password Hashing**
Current implementation uses SHA256 - consider upgrading to:
```csharp
// Use BCrypt or PBKDF2
using BCrypt.Net;

string hashedPassword = BCrypt.HashPassword(plainPassword);
bool isValid = BCrypt.Verify(plainPassword, hashedPassword);
```

### **4. Input Validation**
```csharp
// Always validate and sanitize user input
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "Name,Email")] Customer customer)
{
    if (ModelState.IsValid)
    {
        // Process
    }
}
```

### **5. SQL Injection Prevention**
? Already using parameterized queries in DAL
```csharp
// Good - using SqlParameter
SqlParameter[] parameters = {
    new SqlParameter("@ProductID", productId),
    new SqlParameter("@Quantity", quantity)
};
```

---

## ?? Quick Deployment Checklist

### **Local Development (IIS Express)**
- [ ] SQL Server running (Docker or local)
- [ ] Database schema imported
- [ ] Connection string configured
- [ ] Run from Visual Studio (F5)
- [ ] Access at `https://localhost:44395/`

### **Production Deployment**
- [ ] Choose hosting option (Azure/IIS/Docker)
- [ ] Provision database (Azure SQL/SQL Server)
- [ ] Import web-optimized schema
- [ ] Update connection strings
- [ ] Configure HTTPS/SSL
- [ ] Set up authentication
- [ ] Configure firewall rules
- [ ] Test all functionality
- [ ] Monitor logs and performance

---

## ?? Recommended Deployment Path

### **For Beginners**
1. Use **Azure App Service** + **Azure SQL Database**
2. Deploy via Visual Studio Publish
3. Minimal configuration required

### **For Custom Requirements**
1. Use **IIS** on Windows Server + **SQL Server**
2. Full control over configuration
3. Requires server management

### **For Modern/Scalable**
1. Use **Docker Compose** locally
2. Deploy to **Azure Container Instances** or **Kubernetes**
3. Easy to scale and maintain

---

## ?? Support & Resources

- **Azure Documentation**: https://docs.microsoft.com/azure
- **IIS Configuration**: https://learn.microsoft.com/iis
- **Docker Documentation**: https://docs.docker.com
- **ASP.NET MVC**: https://learn.microsoft.com/aspnet/mvc

---

## ? Next Steps

1. ? Choose deployment platform
2. ? Set up database (use web-optimized schema)
3. ? Configure connection strings properly
4. ? Test locally with Docker
5. ? Deploy to production
6. ? Configure monitoring and backups

**Your AWE Electronics project is ready for the web!** ??
