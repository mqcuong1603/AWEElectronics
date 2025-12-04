# ? AWE Electronics Database - Setup Complete!

## ?? Your SQL Server is Running!

```
Container: sqlserver2022
Status:    ? RUNNING
Database:  ? AWEElectronics_DB
Tables:    ? 8 tables created
Data:      ? Sample data loaded
```

---

## ?? Connection Information

### For SQL Server Management Studio (SSMS)

```
Server:         localhost,1433
Authentication: SQL Server Authentication
Login:          sa
Password:       YourStrong@Password123
Database:       AWEElectronics_DB
```

### For .NET Application (Connection String)

```csharp
Server=localhost,1433;Database=AWEElectronics_DB;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;
```

---

## ?? Test User Accounts

| Username | Password | Role | Email |
|----------|----------|------|-------|
| `admin` | `admin123` | Admin | admin@aweelectronics.com |
| `manager` | `manager123` | Manager | manager@aweelectronics.com |
| `staff` | `staff123` | Staff | staff@aweelectronics.com |

---

## ?? Database Contents

? **Users:** 3 accounts
? **Categories:** 11 product categories
? **Suppliers:** 3 vendors
? **Products:** 14 sample products

### Tables Created:
- Users
- Categories
- Suppliers
- Products
- Orders
- OrderDetails
- Payments
- InventoryTransactions

---

## ?? Docker Quick Commands

```powershell
# View container status
docker ps

# View logs
docker logs sqlserver2022

# Stop container
docker stop sqlserver2022

# Start container
docker start sqlserver2022

# Restart container
docker restart sqlserver2022
```

---

## ?? Test Query (Copy & Paste)

```sql
-- Test connection and view products
USE AWEElectronics_DB;
GO

SELECT TOP 5 
    p.ProductID,
    p.SKU,
    p.Name,
    p.Price,
    p.StockLevel,
    c.Name AS Category,
    s.CompanyName AS Supplier
FROM Products p
LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
ORDER BY p.ProductID;
```

---

## ?? Quick Test in PowerShell

```powershell
# Run a quick query
docker exec sqlserver2022 /opt/mssql-tools18/bin/sqlcmd `
    -S localhost -U sa -P "YourStrong@Password123" -C `
    -d AWEElectronics_DB `
    -Q "SELECT COUNT(*) AS TotalProducts FROM Products"
```

---

## ?? Next Steps

1. **Open SSMS** and connect using the info above
2. **Test your application** with the connection string
3. **Browse the sample data** to understand the schema
4. **Start developing!**

---

## ?? Tips

- Container will auto-start with Docker Desktop
- Data persists in Docker volume (survives container restart)
- Run `.\test-connection.ps1` anytime to verify status
- Check `DATABASE_SETUP.md` for full documentation

---

## ?? Security Note

**These credentials are for DEVELOPMENT ONLY!**

Before production:
- Change the SA password
- Create application-specific database users
- Use environment variables for credentials
- Enable SSL/TLS encryption

---

**Setup Date:** 2025-12-04
**SQL Server Version:** Microsoft SQL Server 2022 (RTM-CU22)
**Container:** sqlserver2022

? **Everything is ready! Happy coding!** ??
