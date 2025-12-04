# ?? Connection Reset Error - Application Not Running

## **Error:** ERR_CONNECTION_RESET - This site can't be reached

### **Root Cause:** Your ASP.NET MVC application is not running!

---

## ? **Quick Fix - Start Your Application**

### **Method 1: Press F5 in Visual Studio (Recommended)**

1. **Open Visual Studio**
2. **Open Solution:** `AWEElectronics.sln`
3. **Right-click** on **Web** project in Solution Explorer
4. **Select:** "Set as StartUp Project" (if not already bold)
5. **Press F5** (or click green ? button that says "IIS Express")
6. **Wait** for browser to open automatically
7. **You should see** the Login page

### **Method 2: Run Without Debugging**

1. Press **Ctrl + F5**
2. Browser opens automatically
3. Login page appears

---

## ?? **How to Verify Application is Running**

### **Check 1: Visual Studio Output**
When application starts, you should see in Output window:
```
'iisexpress.exe' (CLR v4.0.30319: /LM/W3SVC/2/ROOT-1-133XXX): Loaded...
Application Started
```

### **Check 2: System Tray**
Look for **IIS Express** icon in system tray (bottom-right corner)
- Should show: "IIS Express - Web (Running)"

### **Check 3: Browser Opens Automatically**
- Browser should open to: `http://localhost:44395/` (or similar port)
- Login page appears

### **Check 4: Task Manager**
1. Open Task Manager (Ctrl+Shift+Esc)
2. Look for process: **iisexpress.exe**
3. If not found ? Application is NOT running

---

## ?? **Common Startup Issues**

### **Issue 1: Wrong Startup Project**

**Symptom:** Desktop application opens instead of web browser

**Solution:**
1. In Solution Explorer, right-click **Web** project
2. Select "Set as StartUp Project"
3. The **Web** project name should now be **bold**
4. Press F5 again

### **Issue 2: Port Already in Use**

**Symptom:** 
```
Unable to launch IIS Express
Port 44395 is already in use
```

**Solution:**
```powershell
# Run in PowerShell as Administrator
Get-Process -Name iisexpress | Stop-Process -Force
```

Then restart application (F5)

### **Issue 3: Build Errors**

**Symptom:** Application won't start, error list shows errors

**Solution:**
1. Build ? Rebuild Solution (Ctrl+Shift+B)
2. Fix any compilation errors
3. Ensure: **0 Errors**
4. Try F5 again

### **Issue 4: SSL Certificate Issue**

**Symptom:** 
```
Unable to start debugging
The SSL certificate could not be validated
```

**Solution:**
1. Tools ? Options ? Projects and Solutions ? Web Projects
2. Check: "Use the 64-bit version of IIS Express"
3. Trust the certificate if prompted
4. Restart Visual Studio

### **Issue 5: IIS Express Not Responding**

**Symptom:** Browser opens but shows loading forever

**Solution:**
```powershell
# Kill all IIS Express processes
Get-Process -Name iisexpress -ErrorAction SilentlyContinue | Stop-Process -Force

# Delete IIS Express configuration
Remove-Item "$env:USERPROFILE\Documents\IISExpress\config\applicationhost.config" -ErrorAction SilentlyContinue
```

Then restart Visual Studio and press F5

---

## ?? **Startup Checklist**

Before accessing any URL, verify:

- [ ] Visual Studio is open
- [ ] Solution is loaded (`AWEElectronics.sln`)
- [ ] **Web** project is set as StartUp Project (bold in Solution Explorer)
- [ ] Solution builds successfully (0 errors)
- [ ] Press F5 or click ? IIS Express
- [ ] Wait for browser to open automatically
- [ ] IIS Express icon appears in system tray
- [ ] Login page loads successfully

---

## ?? **Correct Startup Sequence**

### **Step-by-Step:**

```
1. Open Visual Studio
   ?
2. Open AWEElectronics.sln
   ?
3. Solution Explorer ? Right-click "Web" ? Set as StartUp Project
   ?
4. Press F5 (Start Debugging)
   ?
5. Visual Studio Output shows: "Application started"
   ?
6. Browser opens automatically to: http://localhost:44395/
   ?
7. Login page appears
   ?
8. ? Application is RUNNING
```

---

## ?? **Manual URL Check (After Starting)**

Once application is running, you can access:

| URL | Expected Result |
|-----|----------------|
| `http://localhost:44395/` | Login page |
| `http://localhost:44395/Account/Login` | Login page |
| `http://localhost:44395/Diagnostics/TestConnection` | Diagnostic report (plain text) |

**Important:** Replace `44395` with your actual port number if different!

---

## ?? **Still Getting Connection Reset?**

### **Check 1: Verify Port Number**

Your actual port might be different. Check in:
1. Solution Explorer ? Web project ? Properties
2. Or look at the URL when Visual Studio starts the browser

Common ports:
- `http://localhost:44395/` (HTTPS)
- `http://localhost:12345/` (HTTP)
- Port number varies by project

### **Check 2: Firewall Blocking**

Temporarily disable firewall:
1. Windows Security ? Firewall & network protection
2. Turn off for Private network
3. Try F5 again
4. Re-enable firewall after testing

### **Check 3: Antivirus Blocking**

Some antivirus software blocks localhost:
1. Temporarily disable antivirus
2. Try F5 again
3. Add Visual Studio and IIS Express to antivirus exceptions

### **Check 4: Wrong Browser**

Try a different browser:
1. In Visual Studio: Debug ? Web ? Select Browser
2. Choose different browser (Chrome, Edge, Firefox)
3. Press F5

---

## ?? **Application Status Verification**

### **PowerShell Commands:**

```powershell
# Check if IIS Express is running
Get-Process -Name iisexpress -ErrorAction SilentlyContinue

# Output:
# If running: Shows process details
# If not running: No output (ERROR: Application is NOT running!)

# Check what ports are in use
netstat -ano | findstr "44395"

# Output:
# If running: TCP    127.0.0.1:44395    LISTENING
# If not running: No output
```

### **Visual Studio Diagnostics:**

1. Debug ? Windows ? Output
2. Select "Debug" from Show output from dropdown
3. Look for:
   - "Application started successfully"
   - "Loaded modules..."
   - "IIS Express is running"

---

## ?? **Your Specific Issue**

Based on your screenshot:
- URL: `localhost:44395/Diagnostics/TestConnection`
- Error: ERR_CONNECTION_RESET

**Diagnosis:** Your web application is **NOT RUNNING**

**Solution:**
1. Go back to Visual Studio
2. Press **F5** to start the application
3. Wait for browser to open
4. Then navigate to diagnostics or login

---

## ?? **Quick Start Command**

### **Option 1: Visual Studio**
```
F5 = Start with debugging
Ctrl+F5 = Start without debugging
```

### **Option 2: Command Line (Advanced)**

```powershell
# Navigate to Web project directory
cd "D:\SE\final\AWEElectronics\Web"

# Start IIS Express manually
"C:\Program Files\IIS Express\iisexpress.exe" /path:"D:\SE\final\AWEElectronics\Web" /port:44395
```

---

## ? **Success Indicators**

You'll know the application is running when you see:

1. **Visual Studio:**
   - Output window shows "Application started"
   - Debug toolbar is active (pause, stop buttons enabled)
   - Status bar shows "Running - localhost:44395"

2. **System Tray:**
   - IIS Express icon visible
   - Tooltip shows "IIS Express - Web (Running)"

3. **Browser:**
   - Opens automatically
   - Shows AWE Electronics login page
   - No "can't be reached" error

4. **Task Manager:**
   - Process "iisexpress.exe" is running
   - Shows memory usage and CPU activity

---

## ?? **Still Need Help?**

If application still won't start:

1. **Check Build Output:**
   ```
   Build ? Rebuild Solution
   Check Error List for compilation errors
   ```

2. **Check Event Viewer:**
   ```
   Windows Event Viewer ? Windows Logs ? Application
   Look for IIS Express errors
   ```

3. **Reset IIS Express:**
   ```powershell
   # Delete configuration
   Remove-Item "$env:USERPROFILE\Documents\IISExpress" -Recurse -Force
   
   # Restart Visual Studio
   # Try F5 again
   ```

4. **Repair Visual Studio:**
   ```
   Visual Studio Installer ? More ? Repair
   ```

---

## ?? **Summary**

### **The Problem:**
- You're trying to access `http://localhost:44395/Diagnostics/TestConnection`
- Getting ERR_CONNECTION_RESET error
- **Root cause:** Application is not running!

### **The Solution:**
1. Open Visual Studio
2. Open AWEElectronics.sln
3. Set Web project as StartUp Project
4. Press F5
5. Wait for browser to open
6. Application is now running!

### **Then You Can:**
- Login with admin/password123
- Access diagnostics at /Diagnostics/TestConnection
- Use all features of the application

---

**Status:** ?? **APPLICATION NOT RUNNING**

**Action Required:** Start the application by pressing F5 in Visual Studio!
