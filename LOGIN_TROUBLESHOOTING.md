# Login Troubleshooting Guide

## ? Issue: "Invalid username or password" Error

### **Common Reasons Why Login Fails:**

---

## 1. **No User with Username "account"**

### ? Problem:
You're trying to login with username `account`, but this user doesn't exist in your database.

### ? Valid Test Usernames:
According to your Login page, the valid test usernames are:

| Username | Password | Role |
|----------|----------|------|
| `admin` | `password123` | Admin |
| `jsmith` | `password123` | Staff |
| `bwilson` | `password123` | Agent |

### Solution:
Use one of the valid usernames above, **NOT** "account".

---

## 2. **Case Sensitivity** (Now Fixed!)

### ? Before:
- Typing `Admin` instead of `admin` would fail
- SQL query was case-sensitive: `WHERE Username = @Username`

### ? After (Fixed):
- Now accepts any case: `admin`, `Admin`, `ADMIN` all work
- SQL query is case-insensitive: `WHERE LOWER(Username) = LOWER(@Username)`

**What Changed:**
```sql
-- BEFORE (Case-Sensitive)
SELECT * FROM Users WHERE Username = @Username

-- AFTER (Case-Insensitive)  
SELECT * FROM Users WHERE LOWER(Username) = LOWER(@Username)
```

---

## 3. **Extra Spaces**

### ? Problem:
Typing `" admin "` (with spaces) would fail

### ? Solution (Now Fixed):
Username is automatically trimmed:
```csharp
username = username.Trim();
```

---

## 4. **Wrong Password**

### ? Problem:
Password must be exactly: `password123`

### Common Mistakes:
- ? `Password123` (capital P)
- ? `password` (missing 123)
- ? `password1234` (extra digit)
- ? `password123` (correct)

**Note:** Passwords ARE case-sensitive and must be exact.

---

## ?? How to Diagnose Login Issues

### **Step 1: Check Username**
```
Valid usernames (from your database):
- admin
- jsmith  
- bwilson

NOT valid:
- account (doesn't exist)
- user (doesn't exist)
- test (doesn't exist)
```

### **Step 2: Check Password**
```
All test accounts use the same password:
- password123

Remember:
- Case-sensitive (must be lowercase)
- No spaces
- Must include "123" at the end
```

### **Step 3: Check Account Status**
If you see: `"Account is Inactive"` or `"Account is Locked"`, contact the administrator to activate your account.

---

## ??? What We Fixed

### **1. Case-Insensitive Username Lookup**

**File:** `DAL\UserDAL.cs`

**Change:**
```csharp
// BEFORE
string query = "SELECT * FROM Users WHERE Username = @Username";

// AFTER  
string query = "SELECT * FROM Users WHERE LOWER(Username) = LOWER(@Username)";
```

**Effect:** Now you can type `admin`, `Admin`, or `ADMIN` - all will work!

---

### **2. Trim Whitespace**

**File:** `BLL\UserBLL.cs`

**Change:**
```csharp
// Added this line
username = username.Trim();
```

**Effect:** Extra spaces before/after username are removed automatically.

---

### **3. Better Error Messages**

**Changed:**
```csharp
// BEFORE
Message = "Invalid username or password."

// AFTER
Message = "Invalid username or password. Please check your credentials and try again."
```

---

## ? How to Login Successfully

### **Method 1: Type Credentials Manually**

1. Go to Login page
2. **Username:** Type `admin` (lowercase is fine)
3. **Password:** Type `password123` (must be exact)
4. Click **"Login to Dashboard"**

### **Method 2: Copy-Paste from Credential Table**

1. Look at the "Test Credentials" section on Login page
2. Copy username: `admin`
3. Copy password: `password123`
4. Paste into respective fields
5. Click **"Login to Dashboard"**

---

## ?? Test All Accounts

### **Test 1: Admin Account**
```
Username: admin
Password: password123
Expected: ? Redirect to Dashboard
Role: Admin
```

### **Test 2: Staff Account**
```
Username: jsmith
Password: password123
Expected: ? Redirect to Dashboard
Role: Staff
```

### **Test 3: Agent Account**
```
Username: bwilson
Password: password123
Expected: ? Redirect to Dashboard
Role: Agent
```

### **Test 4: Case Variations (Now Works!)**
```
Username: ADMIN (uppercase)
Password: password123
Expected: ? Should work now!

Username: Admin (mixed case)
Password: password123
Expected: ? Should work now!
```

### **Test 5: Invalid Username**
```
Username: account
Password: password123
Expected: ? "Invalid username or password"
Reason: User "account" doesn't exist
```

### **Test 6: Invalid Password**
```
Username: admin
Password: wrongpassword
Expected: ? "Invalid username or password"
```

---

## ?? Database Check

### **To verify users exist in your database:**

Run this SQL query:
```sql
SELECT Username, Role, Status, CreatedDate 
FROM Users 
ORDER BY Username;
```

**Expected Results:**
```
Username  | Role  | Status | CreatedDate
----------|-------|--------|-------------
admin     | Admin | Active | 2024-XX-XX
bwilson   | Agent | Active | 2024-XX-XX
jsmith    | Staff | Active | 2024-XX-XX
```

If you don't see these users, you need to run the database initialization script to create them.

---

## ?? Password Hashing

### **How Passwords Are Stored:**
- Passwords are hashed using SHA256
- `password123` is hashed to: `ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f`

### **How Login Works:**
1. User types password: `password123`
2. System hashes it: SHA256(`password123`)
3. Compares with stored hash
4. If match ? Login success ?
5. If no match ? Login failed ?

---

## ?? Still Having Issues?

### **Check These:**

1. **Database Connection**
   ```
   Error: "Cannot connect to database"
   Solution: Check Web.config connection string
   ```

2. **No Users in Database**
   ```
   Error: Always "Invalid username"
   Solution: Run database initialization script
   ```

3. **Wrong Database**
   ```
   Error: Users don't exist
   Solution: Check connection string points to AWEElectronics_DB
   ```

4. **Account Inactive**
   ```
   Error: "Account is Inactive"
   Solution: Update user status in database:
   UPDATE Users SET Status = 'Active' WHERE Username = 'admin'
   ```

---

## ?? Quick Reference

### **Valid Login Combinations:**

```
? admin / password123
? Admin / password123 (now works!)
? ADMIN / password123 (now works!)
? jsmith / password123
? bwilson / password123

? account / password123 (user doesn't exist)
? admin / Password123 (wrong password - case matters)
? admin / password (wrong password - missing 123)
```

---

## ?? Summary

### **Why "account" Failed:**
1. ? No user named "account" exists in database
2. ? Valid usernames are: admin, jsmith, bwilson

### **What We Fixed:**
1. ? Made username lookup case-insensitive
2. ? Added automatic whitespace trimming
3. ? Improved error messages

### **How to Login:**
1. ? Use: `admin` / `password123`
2. ? Or: `jsmith` / `password123`
3. ? Or: `bwilson` / `password123`

---

**Status:** ? **FIXED!** Username lookup is now case-insensitive, and you have clear guidance on valid credentials.
