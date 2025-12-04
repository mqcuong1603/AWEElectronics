# ?? QUICK START - Test Role-Based Dashboards NOW!

## ? 5-MINUTE TESTING GUIDE

Follow these steps **EXACTLY** to see different dashboards for each role.

---

## ?? BEFORE YOU START

### ? Run Verification Script (Optional but Recommended)

```powershell
# Open PowerShell in project directory
cd "D:\SE\final\AWEElectronics"

# Run verification script
.\verify-dashboards.ps1
```

This will check:
- ? All dashboard files exist
- ? Database is accessible
- ? Users have correct roles
- ? Controller is configured correctly

---

## ?? STEP 1: RESTART APPLICATION (MANDATORY!)

### In Visual Studio:

1. **STOP the application:**
   - Press **Shift+F5** or click red ? stop button
   - Wait until Output window shows app stopped

2. **Clean and Rebuild:**
   - Menu: **Build** ? **Clean Solution**
   - Wait for completion
   - Menu: **Build** ? **Rebuild Solution**
   - Verify: **0 Errors, 0 Warnings**

3. **START fresh:**
   - Press **F5** or click green ? start button
   - Wait for browser to open automatically

---

## ?? STEP 2: CLEAR BROWSER CACHE (MANDATORY!)

### Quick Method (Works for any browser):

```
1. Press: Ctrl+Shift+Delete
2. Select: "Cached images and files"
3. Time range: "All time"
4. Click: "Clear data"
5. Close ALL browser tabs/windows
6. Go back to Visual Studio
7. Press F5 again
```

---

## ?? STEP 3: TEST ADMIN DASHBOARD

### Login:
```
URL: http://localhost:44395/
Username: admin
Password: password123
```

### ? What You MUST See:

```
???????????????????????????????????????
? ?? ADMIN DASHBOARD                   ? ? RED BACKGROUND
? You are viewing the Administrator   ?
? control panel                       ?
???????????????????????????????????????
```

### Count:
- **Buttons:** 4 buttons (including "Manage Users")
- **Cards:** 4 statistics cards
- **Color:** Red/Purple theme

### ? SUCCESS: You see RED alert box at top!
### ? FAIL: You see default blue navbar (wrong view!)

---

## ?? STEP 4: TEST STAFF DASHBOARD

### Logout and Login:
1. Click user icon (top right)
2. Click "Logout"
3. Login with:
   ```
   Username: jsmith
   Password: password123
   ```

### ? What You MUST See:

```
???????????????????????????????????????
? ?? STAFF DASHBOARD                   ? ? GREEN BACKGROUND
? You are viewing the Staff operations?
? panel                               ?
???????????????????????????????????????
```

### Count:
- **Buttons:** 3 buttons (NO "Manage Users")
- **Cards:** 3 statistics cards
- **Color:** Green theme
- **Unique:** Large "Pending Orders" table

### ? SUCCESS: You see GREEN alert box at top!
### ? FAIL: Still see red or default view!

---

## ?? STEP 5: TEST AGENT DASHBOARD

### Logout and Login:
1. Click user icon (top right)
2. Click "Logout"
3. Login with:
   ```
   Username: bwilson
   Password: password123
   ```

### ? What You MUST See:

```
???????????????????????????????????????
? ?? AGENT DASHBOARD                   ? ? YELLOW BACKGROUND
? You are viewing the Sales Agent     ?
? panel                               ?
???????????????????????????????????????
```

### Count:
- **Buttons:** 2 large buttons
- **Cards:** 4 statistics cards (different labels)
- **Color:** Yellow/Orange theme
- **Unique:** "Top Products" table with medals ??????

### ? SUCCESS: You see YELLOW alert box at top!
### ? FAIL: Still see red/green or default view!

---

## ? SUCCESS CRITERIA

### You know it's working when you see:

1. **Different colored alert boxes:**
   - Admin = ?? RED
   - Staff = ?? GREEN
   - Agent = ?? YELLOW

2. **Different button counts:**
   - Admin = 4 buttons
   - Staff = 3 buttons
   - Agent = 2 buttons

3. **Different unique sections:**
   - Admin = "ADMIN CONTROLS" header (red)
   - Staff = "PENDING ORDERS" table (large)
   - Agent = "TOP PRODUCTS" table (with medals)

---

## ? IF IT'S NOT WORKING

### Problem: All three still look the same

**Try this fix sequence:**

```powershell
# 1. Kill IIS Express
Get-Process -Name iisexpress -ErrorAction SilentlyContinue | Stop-Process -Force

# 2. Delete bin/obj folders
Remove-Item "D:\SE\final\AWEElectronics\Web\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "D:\SE\final\AWEElectronics\Web\obj" -Recurse -Force -ErrorAction SilentlyContinue

# 3. Close ALL browser windows
```

Then:
1. Go to Visual Studio
2. Build ? Clean Solution
3. Build ? Rebuild Solution
4. Press F5
5. Clear browser cache again (Ctrl+Shift+Delete)
6. Try logging in

---

### Problem: Seeing error message

If you see "Error loading dashboard data" - **THIS IS OK!**

The colored alert box and role-specific buttons should still show!

---

### Problem: Not sure which view is loading

**Check Visual Studio Output Window:**

1. Menu: View ? Output
2. Look for one of these lines after login:
   ```
   >>> RENDERING ADMIN DASHBOARD <<<
   >>> RENDERING STAFF DASHBOARD <<<
   >>> RENDERING AGENT DASHBOARD <<<
   ```

If you see "WARNING: Unknown role" - the session isn't set correctly.

---

## ?? VISUAL PROOF

Take screenshots of each dashboard and compare:

### **Admin Screenshot Should Show:**
- Red alert box at top
- 4 buttons in controls section
- "Manage Users" button (unique!)
- Red header on section

### **Staff Screenshot Should Show:**
- Green alert box at top
- 3 buttons (no "Manage Users")
- Large "Pending Orders" table (unique!)
- Green header on section

### **Agent Screenshot Should Show:**
- Yellow alert box at top
- 2 large buttons only
- Blue info box saying "view-only" (unique!)
- "Top Products" table with medals (unique!)
- Yellow header on section

---

## ?? TIMING

This whole process should take **5 minutes:**

- ?? 1 min: Restart app + clear cache
- ?? 1 min: Test admin dashboard
- ?? 1 min: Test staff dashboard
- ?? 1 min: Test agent dashboard
- ?? 1 min: Verify differences

---

## ?? THE EASIEST TEST

**Just look at the alert box color at the very top of the page!**

- See ?? RED = Admin dashboard ?
- See ?? GREEN = Staff dashboard ?
- See ?? YELLOW = Agent dashboard ?
- See no colored box = Default dashboard ? (not working!)

**If you see three different colored boxes, IT'S WORKING!** ??

---

## ?? NEED HELP?

If dashboards still look the same after:
1. ? Restarting app
2. ? Clearing cache
3. ? Rebuilding solution
4. ? Trying force-clear (deleting bin/obj)

Then check:
- Visual Studio Output window for "RENDERING X DASHBOARD" messages
- Database roles are correct: `SELECT Username, Role FROM Users`
- View files exist: `verify-dashboards.ps1`
- Controller has switch statement with View("DashboardAdmin"), etc.

---

## ?? FINAL CHECKLIST

Before you say "it's not working":

- [ ] Did you restart the app? (Shift+F5, then F5)
- [ ] Did you clear browser cache? (Ctrl+Shift+Delete)
- [ ] Did you try all three roles? (admin, jsmith, bwilson)
- [ ] Are you looking at the TOP of the page for colored boxes?
- [ ] Did you close ALL browser windows before restarting?

**If all checked YES and still not working, run `verify-dashboards.ps1` to diagnose!**

---

**Ready? Let's go! Press F5 and start testing!** ??
