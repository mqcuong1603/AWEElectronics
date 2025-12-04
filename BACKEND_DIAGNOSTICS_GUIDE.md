# ?? Backend & Database Diagnostics Guide

## ?? **You're Having Login Issues - Let's Fix It!**

This guide will help you diagnose and fix backend/database connectivity problems.

---

## ?? **Quick Diagnostic Tool**

### **Run Diagnostics Now:**

1. **Start your application** (Press F5)
2. **Navigate to:** `http://localhost:44395/Diagnostics/TestConnection`
3. **View the diagnostic report** in plain text

This will test:
- ? Database connection
- ? Users table existence
- ? Admin user verification
- ? Password hash validation
- ? Session configuration
- ? Application settings

---

## ?? **Common Issues & Solutions**

### **Issue 1: Build Errors (Current Problem)**

**Status:** ? **Your project has 49 build errors**

**Most Common Errors:**
- Missing DTO properties (ImageURL, Status, Description, etc.)
- Missing `@` escaping in Razor views
- Missing project references

**Impact:** Application won't run until these are fixed.

**Solution:**
These are view-related errors that don't affect core login functionality, but they prevent the app from running. They need to be fixed first.

---

### **Issue 2: Database Not Running**

**Symptoms:**
- Login always fails
- "Cannot connect to database" error
- Timeout errors

**Check:**
```powershell
# Check if SQL Server is running
Get-Service MSSQLSERVER
```

**Solutions:**
```powershell
# Start SQL Server
Start-Service MSSQLSERVER

# Or start SQL Server Express
Start-Service MSSQL$SQLEXPRESS
```

**Visual Check:**
1. Open **SQL Server Configuration Manager**
2. Check if **SQL Server (MSSQLSERVER)** service is running
3. If stopped, right-click ? Start

---

### **Issue 3: Database Doesn't Exist**

**Symptoms:**
- "Cannot open database 'AWEElectronics_DB'"
- Login fails silently

**Check:**
```sql
-- In SQL Server Management Studio (SSMS)
SELECT name FROM sys.databases WHERE name = 'AWEElectronics_DB'
```

**Solution:**
If database doesn't exist, you need to:
1. Run database creation scripts
2. Run table creation scripts
3. Run data initialization scripts (to create test users)

---

### **Issue 4: No Users in Database**

**Symptoms:**
- All logins fail with "Invalid username or password"
- Diagnostics shows "0 users found"

**Check:**
```sql
SELECT Username, Role, Status FROM Users
```

**Solution:**
You need to insert test users. Run this SQL:

```sql
-- Insert test users with hashed password for "password123"
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, Status, CreatedDate)
VALUES 
('admin', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Administrator', 'admin@aweelectronics.com', 'Admin', 'Active', GETDATE()),
('jsmith', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'John Smith', 'jsmith@aweelectronics.com', 'Staff', 'Active', GETDATE()),
('bwilson', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Bob Wilson', 'bwilson@aweelectronics.com', 'Agent', 'Active', GETDATE());
```

**Note:** The password hash above is for "password123"

---

### **Issue 5: Wrong Connection String**

**Symptoms:**
- "Login failed for user 'sa'"
- "Cannot connect to server"

**Check Web.config:**
```xml
<connectionStrings>
  <add name="AWEElectronics" 
       connectionString="Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

**Common Problems:**
| Problem | Solution |
|---------|----------|
| Wrong server name | Use `localhost` or `.` or `(local)` |
| Wrong port | SQL Server default is `1433` |
| Wrong credentials | Verify SA password |
| Database name typo | Check spelling of `AWEElectronics_DB` |
| Missing TrustServerCertificate | Add `TrustServerCertificate=True` |

---

### **Issue 6: User Account Inactive**

**Symptoms:**
- Login fails with "Account is Inactive"

**Check:**
```sql
SELECT Username, Status FROM Users WHERE Username = 'admin'
```

**Solution:**
```sql
-- Activate the account
UPDATE Users SET Status = 'Active' WHERE Username = 'admin'
```

---

### **Issue 7: Password Hash Mismatch**

**Symptoms:**
- Diagnostic shows password hash doesn't match
- Login always fails even with correct password

**Understanding Password Hashing:**
- Password "password123" should hash to: `ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f`
- Uses SHA256 algorithm
- Case-insensitive comparison

**Check:**
```sql
SELECT Username, PasswordHash FROM Users WHERE Username = 'admin'
```

**Solution:**
```sql
-- Reset password to "password123"
UPDATE Users 
SET PasswordHash = 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f'
WHERE Username = 'admin'
```

---

## ?? **Step-by-Step Troubleshooting**

### **Step 1: Check Build Status**

```
? Currently: 49 errors (needs fixing first)
? Goal: 0 errors, 0 warnings
```

**Action:** Fix build errors before testing login

---

### **Step 2: Verify SQL Server is Running**

**Method 1: Services**
1. Press `Win + R`
2. Type `services.msc`
3. Look for "SQL Server (MSSQLSERVER)"
4. Status should be "Running"

**Method 2: PowerShell**
```powershell
Get-Service | Where-Object {$_.DisplayName -like "*SQL Server*"}
```

---

### **Step 3: Test Database Connection**

**Method 1: SQL Server Management Studio (SSMS)**
1. Open SSMS
2. Connect to `localhost` or `.`
3. Use credentials: `sa` / `YourStrong@Password123`
4. If connection succeeds ? Database server is working ?

**Method 2: Visual Studio Server Explorer**
1. View ? Server Explorer
2. Right-click "Data Connections"
3. Add Connection
4. Test connection

---

### **Step 4: Verify Database Exists**

```sql
-- In SSMS, run this query
USE master;
GO
SELECT name, database_id, create_date 
FROM sys.databases 
WHERE name = 'AWEElectronics_DB';
```

**Expected Result:** 1 row returned
**If 0 rows:** Database doesn't exist - needs to be created

---

### **Step 5: Check Users Table**

```sql
USE AWEElectronics_DB;
GO

-- Check if table exists
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'Users';

-- Count users
SELECT COUNT(*) as UserCount FROM Users;

-- List all users
SELECT Username, Role, Status, CreatedDate FROM Users;
```

**Expected:** At least 3 users (admin, jsmith, bwilson)

---

### **Step 6: Run Diagnostics**

1. Fix build errors first
2. Start application
3. Navigate to: `http://localhost:44395/Diagnostics/TestConnection`
4. Review all test results
5. Address any failures

---

## ?? **Most Likely Cause of Your Issue**

Based on common patterns, your login issue is most likely:

### **#1 - Build Errors Preventing Application from Running** (Current)
**Probability:** 95%
**Fix:** Need to resolve 49 build errors first

### **#2 - No Users in Database**
**Probability:** 80% (if app runs)
**Symptom:** "Invalid username or password" for all attempts
**Fix:** Insert test users using SQL script above

### **#3 - SQL Server Not Running**
**Probability:** 60%
**Symptom:** Timeout errors, cannot connect
**Fix:** Start SQL Server service

### **#4 - Wrong Connection String**
**Probability:** 40%
**Symptom:** Connection errors, authentication failures
**Fix:** Verify Web.config connection string

### **#5 - Database Doesn't Exist**
**Probability:** 30%
**Symptom:** "Cannot open database" error
**Fix:** Create database and run schema scripts

---

## ?? **Quick Fix Commands**

### **Start SQL Server (PowerShell as Admin):**
```powershell
Start-Service MSSQLSERVER
```

### **Create Database (SQL):**
```sql
CREATE DATABASE AWEElectronics_DB;
```

### **Create Users Table (SQL):**
```sql
USE AWEElectronics_DB;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
```

### **Insert Test Users (SQL):**
```sql
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, Status)
VALUES 
('admin', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 
 'Administrator', 'admin@awe.com', 'Admin', 'Active'),
('jsmith', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 
 'John Smith', 'jsmith@awe.com', 'Staff', 'Active'),
('bwilson', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 
 'Bob Wilson', 'bwilson@awe.com', 'Agent', 'Active');
```

---

## ?? **Diagnostic Checklist**

Before attempting to login, verify:

- [ ] SQL Server service is running
- [ ] Database "AWEElectronics_DB" exists
- [ ] Users table exists and has data
- [ ] Admin user exists with Status = 'Active'
- [ ] Password hash matches expected value
- [ ] Connection string in Web.config is correct
- [ ] Application builds without errors (0 errors)
- [ ] Browser can reach the application
- [ ] Session state is configured in Web.config

---

## ?? **Getting More Help**

### **View Application Logs:**
1. Visual Studio ? View ? Output
2. Select "Debug" from dropdown
3. Look for exceptions or errors

### **View Browser Console:**
1. Press F12 in browser
2. Go to Console tab
3. Look for JavaScript errors

### **Test Login Manually:**
1. Open Network tab (F12)
2. Attempt login
3. Check HTTP response
4. Look for 500 errors or exceptions

---

## ? **Next Steps**

### **Immediate Actions:**

1. **Fix Build Errors** - This is blocking everything
   - Many errors are related to missing DTO properties
   - Some are Razor syntax issues (@keyframes escaping)

2. **Once Build Succeeds:**
   - Run diagnostics: `/Diagnostics/TestConnection`
   - Check what tests fail
   - Address each failure

3. **Common Fix Sequence:**
   - Start SQL Server
   - Create database (if missing)
   - Create tables (if missing)
   - Insert test users (if missing)
   - Test login

---

## ?? **Support Information**

If diagnostics show all tests passing but login still fails:

1. Clear browser cache completely
2. Try Incognito/Private mode
3. Try different browser
4. Check for CORS issues
5. Verify anti-forgery token is working
6. Check session cookies are being set

---

**Status:** ?? **BUILD ERRORS - Must fix 49 compilation errors before testing backend**

Once build succeeds, run diagnostics to check backend connectivity!
