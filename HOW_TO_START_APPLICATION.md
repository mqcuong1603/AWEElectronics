# ?? How to Start Your Application - Visual Guide

## ? **Your Current Problem:**

You're seeing this error:
```
ERR_CONNECTION_RESET
This site can't be reached
The connection was reset
```

**Why?** Your application is **NOT RUNNING**! You need to start it first.

---

## ? **Solution: Start the Application in Visual Studio**

### **Step 1: Open Visual Studio**
1. Launch Visual Studio 2022
2. Open the solution file: `AWEElectronics.sln`

### **Step 2: Set Web as Startup Project**
1. Look at Solution Explorer (usually on right side)
2. Find the **Web** project
3. Right-click on **Web**
4. Select **"Set as StartUp Project"**
5. The **Web** project name should now appear in **BOLD**

```
Solution 'AWEElectronics'
??? BLL
??? DAL
??? Desktop
??? DTO
??? Web ? This should be BOLD (StartUp Project)
```

### **Step 3: Start the Application**
Two options:

**Option A: With Debugging (Recommended)**
- Press **F5** on keyboard
- Or click the green ? button that says "IIS Express"

**Option B: Without Debugging**
- Press **Ctrl + F5**
- Faster startup, but no debugging features

### **Step 4: Wait for Browser to Open**
- Visual Studio will:
  1. Build the solution
  2. Start IIS Express
  3. Open your default browser automatically
  4. Navigate to: `http://localhost:44395/`

### **Step 5: Verify Application is Running**
You should see:
- ? Login page with AWE Electronics branding
- ? Purple gradient background
- ? Username and password fields
- ? Test credentials displayed

---

## ?? **What You'll See When It Works**

### **Visual Studio:**
```
Output Window:
========== Build: 5 succeeded, 0 failed ==========
'iisexpress.exe' loaded
Application started
```

### **System Tray:**
- Look for IIS Express icon (bottom-right, near clock)
- Tooltip shows: "IIS Express - Web (Running)"

### **Browser:**
- Opens automatically to login page
- URL: `http://localhost:44395/` or `https://localhost:44395/`
- No error messages

---

## ?? **Troubleshooting**

### **Problem: "Port already in use"**
**Solution:**
```powershell
# Open PowerShell as Administrator
Get-Process -Name iisexpress | Stop-Process -Force
```
Then press F5 again

### **Problem: Desktop app opens instead of web browser**
**Cause:** Desktop project is set as StartUp Project  
**Solution:** Right-click Web project ? Set as StartUp Project ? F5

### **Problem: Build fails with errors**
**Solution:**
1. Build ? Rebuild Solution
2. Fix any errors in Error List
3. Should show: **0 Errors, 0 Warnings**
4. Then press F5

### **Problem: Browser shows "Loading..." forever**
**Solution:**
```powershell
# Kill IIS Express
Get-Process -Name iisexpress -ErrorAction SilentlyContinue | Stop-Process -Force

# Restart application
# Go to Visual Studio ? Press F5
```

---

## ?? **Quick Status Check**

### **Run the PowerShell Script:**
```powershell
# In PowerShell, navigate to project folder
cd "D:\SE\final\AWEElectronics"

# Run status checker
.\check-app-status.ps1
```

This will tell you:
- ? Is IIS Express running?
- ? Is port 44395 listening?
- ? Can connect to application?
- ? Is SQL Server running?

---

## ?? **Understanding the Process**

### **When you press F5:**
```
1. Visual Studio builds all projects
   ?
2. IIS Express process starts
   ?
3. Application loads into IIS Express
   ?
4. Port 44395 starts listening
   ?
5. Browser opens automatically
   ?
6. Application is now accessible
```

### **Application is running when:**
- ? IIS Express icon in system tray
- ? Visual Studio shows debug toolbar (pause, stop buttons)
- ? Output window shows "Application started"
- ? Browser can load the pages

---

## ?? **Your Action Plan**

### **Right Now:**

1. **Close the browser** showing the error
2. **Go to Visual Studio**
3. **Check if application is already running:**
   - Look for IIS Express icon in system tray
   - Check if debug toolbar is active
4. **If NOT running:**
   - Press **F5**
   - Wait for browser to open
5. **If already running:**
   - Just navigate to: `http://localhost:44395/`

### **After Application Starts:**

1. **Login Page** should appear
2. **Test Login:**
   - Username: `admin`
   - Password: `password123`
3. **Should redirect to** Dashboard
4. **Then you can access diagnostics:**
   - Navigate to: `http://localhost:44395/Diagnostics/TestConnection`

---

## ?? **Still Can't Start?**

### **Check These:**

#### **1. Visual Studio Issues**
```
Tools ? Options ? Projects and Solutions ? Web Projects
? Use the 64-bit version of IIS Express for web sites and projects
```

#### **2. Firewall Blocking**
```
Windows Security ? Firewall & network protection
? Allow an app through firewall
? Add: IIS Express
? Check both Private and Public
```

#### **3. IIS Express Corruption**
```powershell
# Delete IIS Express config (run as admin)
Remove-Item "$env:USERPROFILE\Documents\IISExpress\config" -Recurse -Force

# Restart Visual Studio
# Press F5 to regenerate config
```

#### **4. Port Conflict**
Change port in Web project properties:
1. Right-click Web project ? Properties
2. Web tab ? Project URL
3. Change port from 44395 to 44396
4. Save and try F5

---

## ? **Success Checklist**

Before trying to login, verify:

- [ ] Visual Studio is open
- [ ] AWEElectronics.sln is loaded
- [ ] Web project is **bold** (StartUp Project)
- [ ] Solution builds (0 errors)
- [ ] Pressed F5
- [ ] IIS Express icon in system tray
- [ ] Browser opened automatically
- [ ] Login page loaded
- [ ] No "ERR_CONNECTION_RESET" error

---

## ?? **Next Steps**

### **Once Application is Running:**

1. **Login:**
   - Username: `admin`
   - Password: `password123`

2. **Test Features:**
   - Dashboard - View stats
   - Products - Browse items
   - Orders - Manage orders
   - Reports - View analytics

3. **Run Diagnostics:**
   - URL: `http://localhost:44395/Diagnostics/TestConnection`
   - Check database connectivity
   - Verify users table
   - Validate password hashes

---

## ?? **Quick Start Command Summary**

```
1. Open Visual Studio
2. Open AWEElectronics.sln
3. Right-click "Web" ? Set as StartUp Project
4. Press F5
5. Wait for browser
6. Login page appears
7. ? DONE!
```

---

**Current Status:** ?? **NOT RUNNING**  
**Required Action:** **Press F5 in Visual Studio!**

Once you start the application, the ERR_CONNECTION_RESET error will disappear and you'll see the login page instead!
