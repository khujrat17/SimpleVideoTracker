# Complete Setup Instructions

## What You Have

A complete Visual Studio solution with:
- ✅ SimpleVideoTracker.sln (Solution file)
- ✅ SimpleVideoTracker.csproj (Project file)
- ✅ All source files (28 files)
- ✅ NuGet packages configuration
- ✅ Database schema
- ✅ Sample data

## Prerequisites

1. **Visual Studio 2019 or 2022**
   - With ASP.NET and web development workload
   - Download: https://visualstudio.microsoft.com/downloads/

2. **MySQL 8.0**
   - Download: https://dev.mysql.com/downloads/installer/
   - Remember your root password!

3. **.NET Framework 4.8**
   - Usually included with Visual Studio

## Step-by-Step Setup

### Step 1: Install MySQL

1. Download MySQL 8.0 installer
2. Run installer
3. Choose "Developer Default" setup
4. Set root password (e.g., "password123")
5. Complete installation

### Step 2: Create Database

Open **MySQL Command Line Client** or **MySQL Workbench**:

```sql
CREATE DATABASE VideoTrackerDB;
```

**OR** run the provided SQL script:

```bash
# Navigate to project folder
cd SimpleVideoTracker

# Run the schema script
mysql -u root -p < database_schema.sql
```

Enter your MySQL root password when prompted.

### Step 3: Open Solution in Visual Studio

1. Double-click **SimpleVideoTracker.sln**
2. Visual Studio will open the solution
3. Wait for solution to load

### Step 4: Restore NuGet Packages

Visual Studio should automatically restore packages. If not:

1. Right-click on solution → **Restore NuGet Packages**
2. Or go to: Tools → NuGet Package Manager → Package Manager Console
3. Run: `Update-Package -reinstall`

**Required packages:**
- Microsoft.AspNet.Mvc (5.2.7)
- MySql.Data (8.0.33)

### Step 5: Update Connection String

1. Open **Web.config** in Visual Studio
2. Find the `<connectionStrings>` section
3. Update the password:

```xml
<connectionStrings>
  <add name="MySqlConnection" 
       connectionString="Server=localhost;Database=VideoTrackerDB;Uid=root;Pwd=YOUR_MYSQL_PASSWORD;" 
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

Replace `YOUR_MYSQL_PASSWORD` with your actual MySQL root password.

### Step 6: Build the Solution

1. Click **Build** → **Build Solution** (or press Ctrl+Shift+B)
2. Check **Output** window for any errors
3. Should see: "Build succeeded"

### Step 7: Run the Application

1. Press **F5** (or click the green "IIS Express" button)
2. Application will:
   - Start IIS Express
   - Create database tables (if not already created)
   - Insert 8 sample videos
   - Create demo user
   - Open your default browser

### Step 8: Login and Test

Browser should open to: `http://localhost:XXXXX/`

**Login with demo account:**
- Email: `demo@test.com`
- Password: `demo123`

**Or create new account:**
- Click "Register here"
- Enter email and password
- Click Register

### Step 9: Explore the Application

1. **Video Library** - See all 8 sample videos
2. **Click a video** - Opens video player
3. **Watch a video** - Progress saves automatically
4. **Go back** - See updated progress bar
5. **Re-open video** - Resumes from last position

## Troubleshooting

### Problem: NuGet Package Errors

**Solution:**
```
1. Tools → NuGet Package Manager → Package Manager Console
2. Run: Install-Package MySql.Data -Version 8.0.33
3. Run: Install-Package Microsoft.AspNet.Mvc -Version 5.2.7
```

### Problem: Cannot Connect to MySQL

**Solution:**
1. Make sure MySQL service is running:
   - Windows: Services → MySQL80 → Start
2. Test connection:
   ```bash
   mysql -u root -p
   ```
3. Verify password in Web.config matches MySQL password

### Problem: Tables Not Created

**Solution:**
1. Check MySQL user has CREATE permissions
2. Manually run database_schema.sql:
   ```bash
   mysql -u root -p < database_schema.sql
   ```
3. Check Output window in Visual Studio for errors

### Problem: Port Already in Use

**Solution:**
1. Right-click project → Properties
2. Click Web tab
3. Change Project URL port number
4. Click "Create Virtual Directory"

### Problem: Build Errors

**Solution:**
1. Clean solution: Build → Clean Solution
2. Rebuild: Build → Rebuild Solution
3. Check all files are included in project
4. Restore NuGet packages again

### Problem: Videos Won't Play

**Solution:**
1. Check browser console (F12) for errors
2. Try different browser (Chrome recommended)
3. Check video URLs are accessible
4. Ensure internet connection (videos are from Google Cloud)

## Project Structure

```
SimpleVideoTracker/
│
├── SimpleVideoTracker.sln          ← Open this file
├── SimpleVideoTracker.csproj       ← Project configuration
├── Web.config                      ← Update MySQL password here
├── Global.asax / Global.asax.cs    ← Application startup
├── packages.config                 ← NuGet packages
├── database_schema.sql             ← Database creation script
│
├── App_Start/
│   └── RouteConfig.cs              ← URL routing
│
├── Controllers/
│   ├── AccountController.cs        ← Login, Register, Logout
│   └── VideoController.cs          ← Video list, Watch, Progress
│
├── Models/
│   ├── User.cs
│   ├── Video.cs
│   ├── UserVideoProgress.cs
│   └── LoginViewModel.cs
│
├── Data/
│   ├── DatabaseHelper.cs           ← Auto-creates tables
│   ├── UserRepository.cs
│   ├── VideoRepository.cs
│   └── ProgressRepository.cs
│
├── Views/
│   ├── _ViewStart.cshtml
│   ├── Web.config
│   ├── Shared/
│   │   └── _Layout.cshtml          ← Main layout
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   └── Video/
│       ├── Index.cshtml            ← Video library
│       └── Watch.cshtml            ← Video player
│
└── Properties/
    └── AssemblyInfo.cs             ← Project info
```

## Verify Installation

### Check 1: Database Created
```sql
mysql -u root -p
SHOW DATABASES;
-- Should see VideoTrackerDB

USE VideoTrackerDB;
SHOW TABLES;
-- Should see Users, Videos, UserVideoProgress
```

### Check 2: Sample Data Loaded
```sql
SELECT COUNT(*) FROM Videos;
-- Should return 8

SELECT * FROM Users WHERE Email = 'demo@test.com';
-- Should return 1 row
```

### Check 3: Application Running
- Browser opens to login page
- Can login with demo@test.com / demo123
- Can see 8 videos in library
- Can click and play videos

## Configuration Options

### Change Port

1. Right-click project → Properties
2. Web tab → Project URL
3. Change port number
4. Save

### Change Database Name

1. MySQL: `CREATE DATABASE MyVideoApp;`
2. Web.config: Update `Database=MyVideoApp`
3. database_schema.sql: Update `USE MyVideoApp`

### Add More Videos

Edit `Data/DatabaseHelper.cs` → `InsertSampleVideos()` method:

```csharp
string insertVideo = 
    "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
    "('My Video', 'Description', 30, 'https://video-url.mp4', 'https://thumb.jpg')";
```

## Development Workflow

1. **Make changes** to code
2. **Save files** (Ctrl+S)
3. **Run** (F5) - IIS Express auto-reloads
4. **Test** in browser
5. **Stop** debugging (Shift+F5)

## Building for Deployment

### Build Release Version

1. Change to Release mode (toolbar dropdown)
2. Build → Publish
3. Choose publish target (Folder, IIS, Azure, etc.)
4. Configure settings
5. Click Publish

### Deploy to IIS

1. Build in Release mode
2. Copy bin, Content, Views, Web.config to server
3. Create IIS website
4. Point to published folder
5. Set application pool (.NET 4.x)
6. Update Web.config with production DB connection

## Next Steps

### Customize the Application

1. **Change colors** - Edit Views/Shared/_Layout.cshtml
2. **Add logo** - Add image and update navbar
3. **Change demo account** - Edit DatabaseHelper.cs
4. **Add more videos** - Edit DatabaseHelper.cs
5. **Modify UI** - Edit .cshtml files

### Enhance Features

1. Add password hashing (BCrypt)
2. Add admin panel
3. Add video upload
4. Add categories
5. Add search
6. Add user profiles

### Deploy to Production

1. Get hosting (Azure, AWS, or shared hosting with IIS)
2. Set up production MySQL database
3. Update connection string
4. Publish from Visual Studio
5. Test thoroughly

## Support

### Documentation Files

- **README.md** - Complete documentation
- **QUICKSTART.md** - Quick setup guide
- **IMPLEMENTATION_SUMMARY.md** - Technical details
- **FILE_LISTING.md** - All files explained
- **SETUP_INSTRUCTIONS.md** - This file

### Common Commands

**Restore packages:**
```
Update-Package -reinstall
```

**Clean build:**
```
Build → Clean Solution
Build → Rebuild Solution
```

**Database check:**
```sql
mysql -u root -p
USE VideoTrackerDB;
SHOW TABLES;
```

## Success Checklist

- [ ] MySQL installed and running
- [ ] Database created (VideoTrackerDB)
- [ ] Solution opens in Visual Studio
- [ ] NuGet packages restored
- [ ] Connection string updated
- [ ] Solution builds without errors
- [ ] Application runs (F5)
- [ ] Browser opens to login page
- [ ] Can login with demo account
- [ ] Can see 8 videos
- [ ] Can watch videos
- [ ] Progress tracking works

## You're Done! 🎉

Your simple video tracker is now ready to use!

**Next:** Start customizing it for your needs or deploy to production.

---

**Need help?** Check README.md for detailed documentation.
