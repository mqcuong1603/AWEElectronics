# AWE Electronics - Quick Reference Card

## ?? Quick Setup (30 seconds)

```powershell
# Run this one command:
.\setup-sqlserver-docker.ps1
```

That's it! The script does everything automatically.

---

## ?? Connection Info

**Server:** `localhost,1433`  
**Database:** `AWEElectronics_DB`  
**Username:** `sa`  
**Password:** `YourStrong@Password123`

---

## ?? Login Credentials

| Username | Password | Role |
|----------|----------|------|
| admin | admin123 | Admin |
| manager | manager123 | Manager |
| staff | staff123 | Staff |

---

## ?? Common Docker Commands

```powershell
# Start container
docker start sqlserver2022

# Stop container
docker stop sqlserver2022

# View logs
docker logs sqlserver2022

# Check status
docker ps
```

---

## ?? Test Connection

```powershell
.\test-database-connection.ps1
```

---

## ?? Database Contents

- ? 8 Tables (Users, Categories, Products, Orders, etc.)
- ? 3 Default users
- ? 11 Product categories
- ? 14 Sample products
- ? 3 Suppliers

---

## ?? Troubleshooting

**Container won't start?**
```powershell
docker restart sqlserver2022
```

**Can't connect?**
1. Wait 30 seconds after starting
2. Check logs: `docker logs sqlserver2022`
3. Restart: `docker restart sqlserver2022`

**Port 1433 in use?**
```powershell
netstat -ano | findstr :1433
```

---

## ?? Important Files

- `setup-sqlserver-docker.ps1` - Automated setup
- `AWE_Electronics_Database.sql` - Database schema
- `test-database-connection.ps1` - Connection test
- `DATABASE_SETUP.md` - Full documentation

---

## ? Next Steps After Setup

1. ? Test connection
2. ? Open solution in Visual Studio
3. ? Build solution
4. ? Run application
5. ? Login with admin/admin123

---

**Need help?** Check `DATABASE_SETUP.md` for detailed documentation.
