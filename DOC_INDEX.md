# ?? AWE Electronics Web Version - Documentation Index

## ?? START HERE: Quick Navigation

### I want to... 

#### ? **Set up and run the web version RIGHT NOW**
? **Run this command:**
```powershell
.\setup-web-version.ps1
```
Then open solution and press F5. Done!

---

#### ?? **Read step-by-step instructions**
? **Read:** `WEB_VERSION_QUICK_START.md`
- Complete setup guide
- Manual steps explained
- Troubleshooting section
- Default accounts

---

#### ? **Check what needs to be done**
? **Read:** `WEB_VERSION_CHECKLIST.md`
- Implementation status
- Your tasks list
- Verification steps
- Feature comparison

---

#### ?? **Understand the architecture visually**
? **Read:** `WEB_VERSION_VISUAL_GUIDE.md`
- Architecture diagrams
- Setup flow charts
- Connection flow
- Database schema visual

---

#### ?? **Deploy to production (Azure/IIS/Docker)**
? **Read:** `Deployment/WEB_DEPLOYMENT_GUIDE.md`
- Azure deployment
- IIS setup
- Docker containerization
- Production configuration

---

#### ??? **Review database schema**
? **Read:** `Database/AWE_Electronics_WebStore_Schema.sql`
- Complete schema definition
- Sample data
- Views and procedures
- Comments and documentation

---

#### ? **Get a quick overview**
? **Read:** `README_WEB_VERSION.md` (this file)
- Quick summary
- Key features
- Common commands
- Success criteria

---

## ?? Complete File List

### ?? Setup & Configuration Files
| File | Purpose | Action |
|------|---------|--------|
| `setup-web-version.ps1` | Automated setup script | **RUN FIRST** |
| `Web/Web.config.production` | Production config template | Review & use |
| `Web/Web.config` | Active configuration | Updated by script |

### ?? Documentation Files
| File | Purpose | When to Read |
|------|---------|--------------|
| `README_WEB_VERSION.md` | Overview & summary | Start here |
| `WEB_VERSION_QUICK_START.md` | Detailed setup guide | Step-by-step instructions |
| `WEB_VERSION_CHECKLIST.md` | Implementation status | What's done/todo |
| `WEB_VERSION_VISUAL_GUIDE.md` | Visual diagrams | Understand architecture |
| `Deployment/WEB_DEPLOYMENT_GUIDE.md` | Production deployment | Before going live |
| `DOC_INDEX.md` | This file | Navigation help |

### ??? Database Files
| File | Purpose | Action |
|------|---------|--------|
| `Database/AWE_Electronics_WebStore_Schema.sql` | **Web-optimized schema** | **Use this** |
| `AWE_Electronics_Database.sql` | Basic schema (legacy) | Don't use for web |

### ?? Code Files (Updated)
| File | What Changed | Impact |
|------|--------------|--------|
| `DAL/DatabaseConfig.cs` | Reads from Web.config | ? Web-ready |
| `DAL/DAL.csproj` | Added System.Configuration | ? Required |

---

## ?? Recommended Reading Order

### For First-Time Setup
1. **`README_WEB_VERSION.md`** (2 min) - Overview
2. **Run:** `.\setup-web-version.ps1` (3 min) - Execute setup
3. **`WEB_VERSION_QUICK_START.md`** (5 min) - Understand what happened
4. **Open Visual Studio ? Press F5** - Test it!

**Total time: ~10 minutes**

---

### For Understanding Architecture
1. **`WEB_VERSION_VISUAL_GUIDE.md`** - See diagrams
2. **`Database/AWE_Electronics_WebStore_Schema.sql`** - Review schema
3. **`DAL/DatabaseConfig.cs`** - See connection logic
4. **`Web/Web.config.production`** - See configuration

---

### For Production Deployment
1. **`WEB_VERSION_CHECKLIST.md`** - Verify setup complete
2. **`Deployment/WEB_DEPLOYMENT_GUIDE.md`** - Choose platform
3. **`Web/Web.config.production`** - Update for production
4. **Deploy & test**

---

## ?? Find Information By Topic

### Setup & Installation
- **Quick setup:** `setup-web-version.ps1` + `README_WEB_VERSION.md`
- **Detailed steps:** `WEB_VERSION_QUICK_START.md`
- **Prerequisites:** `WEB_VERSION_QUICK_START.md` ? "Prerequisites"
- **Troubleshooting:** `WEB_VERSION_QUICK_START.md` ? "Troubleshooting"

### Configuration
- **Connection strings:** `Web/Web.config.production`
- **Database config:** `DAL/DatabaseConfig.cs`
- **Web settings:** `Web/Web.config.production`

### Database
- **Schema definition:** `Database/AWE_Electronics_WebStore_Schema.sql`
- **Sample data:** Same file (bottom section)
- **Views & procedures:** Same file (middle section)
- **Schema visual:** `WEB_VERSION_VISUAL_GUIDE.md`

### Authentication
- **Default accounts:** `WEB_VERSION_QUICK_START.md` ? "Default Accounts"
- **Staff users:** admin, manager, staff, etc.
- **Customer users:** alice.nguyen@email.com, etc.

### Architecture
- **Diagrams:** `WEB_VERSION_VISUAL_GUIDE.md`
- **Connection flow:** `WEB_VERSION_VISUAL_GUIDE.md` ? "Connection Flow"
- **Layer structure:** `WEB_VERSION_VISUAL_GUIDE.md` ? "Architecture Overview"

### Deployment
- **Azure:** `Deployment/WEB_DEPLOYMENT_GUIDE.md` ? "Azure Deployment"
- **IIS:** `Deployment/WEB_DEPLOYMENT_GUIDE.md` ? "IIS Deployment"
- **Docker:** `Deployment/WEB_DEPLOYMENT_GUIDE.md` ? "Docker Deployment"

### Troubleshooting
- **Quick fixes:** `README_WEB_VERSION.md` ? "Troubleshooting Quick Reference"
- **Detailed solutions:** `WEB_VERSION_QUICK_START.md` ? "Troubleshooting"
- **Common issues:** Both files above

---

## ?? Quick Commands Reference

### Setup
```powershell
# Complete automated setup
.\setup-web-version.ps1

# Manual setup (if needed)
.\setup-web-version.ps1 -SkipDocker -UpdateWebConfig $false
```

### Docker
```powershell
# Start SQL Server
docker start sqlserver

# Stop SQL Server
docker stop sqlserver

# Restart SQL Server
docker restart sqlserver

# View logs
docker logs sqlserver

# Remove and start fresh
docker rm -f sqlserver
.\setup-web-version.ps1
```

### Database
```powershell
# Connect to database
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB

# Test connection
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -Q "SELECT 1"

# Count products
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -d AWEElectronics_DB -Q "SELECT COUNT(*) FROM Products"

# Re-import schema
sqlcmd -S localhost,1433 -U sa -P "YourStrong@Password123" -i "Database\AWE_Electronics_WebStore_Schema.sql"
```

### Visual Studio
```
Open: AWEElectronics.sln
Set startup: Right-click Web project ? Set as StartUp Project
Run: Press F5 (debug) or Ctrl+F5 (no debug)
Access: https://localhost:44395/
```

---

## ? Success Verification

Use this checklist after setup:

### Environment
- [ ] Docker Desktop running
- [ ] SQL Server container running (`docker ps`)
- [ ] Database exists (query succeeds)
- [ ] Visual Studio 2019+ installed

### Configuration
- [ ] `Web/Web.config` has connection string
- [ ] `DAL/DAL.csproj` has System.Configuration reference
- [ ] `DAL/DatabaseConfig.cs` updated to read Web.config

### Database
- [ ] AWEElectronics_DB database exists
- [ ] Tables created (Products, Orders, Customers, etc.)
- [ ] Sample data loaded (16 products, 5 users)
- [ ] Views created (vw_ProductCatalog, etc.)
- [ ] Stored procedures created (sp_AddToCart, etc.)

### Application
- [ ] Solution builds without errors
- [ ] Web project is startup project
- [ ] Application runs (F5)
- [ ] Browser opens to https://localhost:44395/
- [ ] Home page loads
- [ ] Login page accessible
- [ ] Can login with admin/admin123
- [ ] Products display correctly

**If all checked: ? You're ready to go!**

---

## ?? Learning Path

### Beginner
1. Run `setup-web-version.ps1`
2. Read `README_WEB_VERSION.md`
3. Open solution and press F5
4. Test login and basic features
5. Explore the application

### Intermediate
1. Read `WEB_VERSION_QUICK_START.md`
2. Review `WEB_VERSION_VISUAL_GUIDE.md`
3. Study `Database/AWE_Electronics_WebStore_Schema.sql`
4. Understand `DAL/DatabaseConfig.cs`
5. Customize features

### Advanced
1. Read `Deployment/WEB_DEPLOYMENT_GUIDE.md`
2. Review `Web/Web.config.production`
3. Plan production deployment
4. Implement security enhancements
5. Deploy to production

---

## ?? Support & Help

### Where to Find Help

| Issue Type | Where to Look |
|------------|---------------|
| Setup problems | `WEB_VERSION_QUICK_START.md` ? Troubleshooting |
| Database issues | `WEB_VERSION_QUICK_START.md` ? Database section |
| Configuration errors | `Web/Web.config.production` comments |
| Deployment questions | `Deployment/WEB_DEPLOYMENT_GUIDE.md` |
| Architecture questions | `WEB_VERSION_VISUAL_GUIDE.md` |
| Quick reference | `README_WEB_VERSION.md` |

### Common Questions

**Q: Which schema file should I use?**
A: Use `Database/AWE_Electronics_WebStore_Schema.sql` (web-optimized)

**Q: Where do I change the connection string?**
A: In `Web/Web.config` under `<connectionStrings>` section

**Q: How do I reset everything?**
A: `docker rm -f sqlserver` then run `.\setup-web-version.ps1` again

**Q: What are the default passwords?**
A: See `WEB_VERSION_QUICK_START.md` ? "Default Accounts" section

**Q: How do I deploy to production?**
A: See `Deployment/WEB_DEPLOYMENT_GUIDE.md`

---

## ?? You're All Set!

Everything you need is documented and ready to use. Start with:

```powershell
.\setup-web-version.ps1
```

Then explore the documentation as needed!

---

## ?? Document Version

- **Created:** January 2025
- **Last Updated:** January 2025
- **Documentation Set:** AWE Electronics Web Version
- **Purpose:** Central navigation for all web version documentation

---

**Happy Developing! ??**
