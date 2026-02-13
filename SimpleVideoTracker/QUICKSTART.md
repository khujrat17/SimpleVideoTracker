# Quick Setup Guide - 5 Steps

## Step 1: Install MySQL

Download MySQL 8.0:
- Windows: https://dev.mysql.com/downloads/installer/
- Set root password during installation (remember it!)

## Step 2: Create Database

Open MySQL Command Line:
```sql
CREATE DATABASE VideoTrackerDB;
```

Or run the included `database_schema.sql` file:
```bash
mysql -u root -p < database_schema.sql
```

## Step 3: Open Project in Visual Studio

1. Open `SimpleVideoTracker.sln` in Visual Studio 2019 or 2022
2. Right-click project → Manage NuGet Packages
3. Install `MySql.Data` package (if not already installed)

## Step 4: Update Connection String

Open `Web.config` and change the password:
```xml
<add name="MySqlConnection" 
     connectionString="Server=localhost;Database=VideoTrackerDB;Uid=root;Pwd=YOUR_PASSWORD;" 
     providerName="MySql.Data.MySqlClient" />
```

## Step 5: Run

Press F5 in Visual Studio!

The application will:
✅ Create tables automatically (if not already created)
✅ Insert 8 sample videos
✅ Create demo user account
✅ Open your browser to the login page

## Login

**Demo Account:**
- Email: demo@test.com
- Password: demo123

Or click "Register here" to create your own account!

## That's It!

You can now:
- Browse the video library
- Watch videos
- Track your progress automatically
- See completion stats

## Troubleshooting

**Can't connect to MySQL?**
```bash
# Test MySQL is running
mysql -u root -p

# Check your password is correct in Web.config
```

**Package restore error?**
```
Right-click solution → Restore NuGet Packages
```

**Port already in use?**
```
Right-click project → Properties → Web
Change port number
```

---

**Need more help? See README.md for detailed documentation.**
