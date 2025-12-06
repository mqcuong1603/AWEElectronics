# Authentication and Order Details Fixes - Summary

## Issues Fixed

### 1. **Authentication System - BCrypt Password Hashing**

**Problem:** The database uses BCrypt password hashing (`$2a$12$...`), but the code was using SHA256 hashing, causing all login attempts to fail.

**Solution:**
- Updated `UserBLL.cs` to use BCrypt for password verification and hashing
- Created `CustomerBLL.cs` for customer authentication with BCrypt
- Created `CustomerDAL.cs` for customer data access
- Updated `LoginResult.cs` to support both User and Customer login
- Downloaded and configured BCrypt.Net-Next v4.0.3 package

**Test Credentials:**
- **Admin Login:** Username: `admin`, Password: `123456`
- **Staff Login:** Username: `jsmith`, Password: `123456`
- **Customer Login:** Email: `alice.nguyen@email.com`, Password: `123456`

All users and customers in the database have password: `123456`

### 2. **Order Details Page - Null Reference Exception**

**Problem:** The Order Details page was throwing a `NullReferenceException` because `Model.PaymentStatus` was null.

**Solution:**
- Updated `OrderDAL.cs` to JOIN with Payments table and retrieve `PaymentStatus`
- Added `CustomerPhone` to the order query for complete customer information
- Updated `MapOrder()` method to populate `PaymentStatus` with default value "Pending" if no payment exists
- Fixed `PaymentDAL.cs` to properly populate alias properties:
  - `PaymentMethod` (alias for `Provider`)
  - `TransactionID` (alias for `TransactionRef`)
  - `PaymentDate` (uses `PaidAt` or current date)
- Fixed `OrderDetailDAL.cs` to use `ProductSKU` column correctly

## Files Modified

### BLL (Business Logic Layer)
- `BLL\UserBLL.cs` - Updated to use BCrypt password hashing
- `BLL\CustomerBLL.cs` - **NEW** - Customer authentication logic
- `BLL\BLL.csproj` - Added BCrypt reference and CustomerBLL
- `BLL\packages.config` - Added BCrypt.Net-Next package

### DAL (Data Access Layer)
- `DAL\CustomerDAL.cs` - **NEW** - Customer data access
- `DAL\OrderDAL.cs` - Added PaymentStatus join and CustomerPhone
- `DAL\PaymentDAL.cs` - Fixed alias property mapping
- `DAL\OrderDetailDAL.cs` - Fixed ProductSKU column mapping
- `DAL\DAL.csproj` - Added CustomerDAL reference

### DTO (Data Transfer Objects)
- `DTO\LoginResult.cs` - Added Customer property for customer login

### Packages
- Downloaded BCrypt.Net-Next v4.0.3 to `packages\BCrypt.Net-Next.4.0.3\`

## Database Schema Notes

The Users table stores staff/admin with these roles:
- Admin
- Staff
- Accountant
- Agent

The Customers table stores customer accounts with email as username.

Both use BCrypt password hash: `$2a$12$LmcMslYkTqLELm0N.F2Wl.vx5N0H9Sq8KJp6pHQnFOg.zRlLJsLKu` (password: 123456)

## Order Status Workflow

1. **Pending** ? Can change to: Processing, Cancelled
2. **Processing** ? Can change to: Shipped, Cancelled
3. **Shipped** ? Can change to: Delivered
4. **Delivered** ? Final status (no changes)
5. **Cancelled** ? Final status (no changes)

## Payment Status Values

- **Pending** - Payment not yet processed
- **Completed** - Payment successful
- **Failed** - Payment failed
- **Refunded** - Payment refunded

## Build Status

? **Build Successful** - All compilation errors resolved

## Next Steps

1. Ensure Visual Studio reloads the BLL and DAL projects with new BCrypt references
2. Test login with the provided credentials
3. Test order details page to verify PaymentStatus displays correctly
4. Verify all CRUD operations work correctly with the new CustomerBLL
