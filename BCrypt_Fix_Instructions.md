# BCrypt Authentication Fix - Manual Steps

## Summary
The database uses BCrypt password hashing ($2a$12$...), but the code was using SHA256. 
This has been fixed by:

1. Created CustomerDAL.cs in DAL project
2. Created CustomerBLL.cs in BLL project  
3. Updated UserBLL.cs to use BCrypt instead of SHA256
4. Updated LoginResult.cs to support both User and Customer login
5. Downloaded BCrypt.Net-Next package to packages folder

## Manual Steps Required in Visual Studio

### Step 1: Add BCrypt Reference to BLL Project
1. In Visual Studio, right-click on BLL project
2. Select "Add" > "Reference"
3. Click "Browse" button
4. Navigate to: D:\AWEElectronics\packages\BCrypt.Net-Next.4.0.3\lib\net48\
5. Select BCrypt.Net-Next.dll and click "Add"

### Step 2: Add CustomerBLL.cs to BLL Project
1. Right-click on BLL project
2. Select "Add" > "Existing Item"
3. Navigate to: D:\AWEElectronics\BLL\CustomerBLL.cs
4. Click "Add"

### Step 3: Add CustomerDAL.cs to DAL Project
1. Right-click on DAL project
2. Select "Add" > "Existing Item"
3. Navigate to: D:\AWEElectronics\DAL\CustomerDAL.cs
4. Click "Add"

### Step 4: Build the Solution
1. Build > Rebuild Solution
2. Fix any remaining errors

## Testing

### Test User Login (Staff/Admin)
- Username: admin
- Password: 123456

All users in the Users table can log in with password: 123456

### Test Customer Login
- Email: alice.nguyen@email.com
- Password: 123456

All customers in the Customers table can log in with password: 123456

## Files Modified
- BLL\UserBLL.cs - Updated to use BCrypt
- BLL\CustomerBLL.cs - NEW FILE for customer authentication
- DAL\CustomerDAL.cs - NEW FILE for customer data access
- DTO\LoginResult.cs - Added Customer property
- BLL\packages.config - Added BCrypt.Net-Next package reference

## Password Hash Format
The database uses BCrypt hash format: $2a$12$LmcMslYkTqLELm0N.F2Wl.vx5N0H9Sq8KJp6pHQnFOg.zRlLJsLKu

This is the hash for password: 123456
