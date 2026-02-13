# Simple Video Tracker - ASP.NET MVC with MySQL

A simple video tracking web application built with classic ASP.NET MVC (.NET Framework 4.8) and MySQL database. No advanced concepts, just basic login and video progress tracking.

## Features

✅ **Simple Login System**
- Basic email/password authentication
- Registration for new users
- Session-based authentication (Forms Authentication)
- Demo account included

✅ **Video Library**
- View all available videos in a card grid
- See video thumbnails and descriptions
- Track watched time per video
- Visual progress bars

✅ **Video Player**
- HTML5 video player
- Automatic progress tracking
- Save progress every minute
- Resume from last position

✅ **Progress Tracking**
- Track how many minutes watched per video
- Mark videos as completed
- See total watch time
- Progress saved automatically

## Technology Stack

- **Framework:** ASP.NET MVC 5 (.NET Framework 4.8)
- **Database:** MySQL 8.0
- **Data Access:** ADO.NET with MySql.Data
- **Authentication:** Forms Authentication (simple session-based)
- **UI:** Bootstrap 5
- **JavaScript:** jQuery

## Database Tables

The application creates 3 simple tables:

### Users
```sql
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

### Videos
```sql
CREATE TABLE Videos (
    VideoId INT PRIMARY KEY AUTO_INCREMENT,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    DurationMinutes INT NOT NULL,
    Url VARCHAR(500) NOT NULL,
    ThumbnailUrl VARCHAR(500),
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

### UserVideoProgress
```sql
CREATE TABLE UserVideoProgress (
    ProgressId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    VideoId INT NOT NULL,
    WatchedMinutes INT DEFAULT 0,
    Completed BOOLEAN DEFAULT FALSE,
    LastWatchedDate DATETIME,
    UNIQUE KEY (UserId, VideoId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (VideoId) REFERENCES Videos(VideoId)
);
```

## Project Structure

```
SimpleVideoTracker/
├── Controllers/
│   ├── AccountController.cs       # Login, Register, Logout
│   └── VideoController.cs         # Video list, Watch, UpdateProgress
├── Models/
│   ├── User.cs                    # User model
│   ├── Video.cs                   # Video model
│   ├── UserVideoProgress.cs       # Progress model
│   └── LoginViewModel.cs          # Login form model
├── Data/
│   ├── DatabaseHelper.cs          # Database connection & initialization
│   ├── UserRepository.cs          # User database operations
│   ├── VideoRepository.cs         # Video database operations
│   └── ProgressRepository.cs      # Progress tracking operations
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml           # Login page
│   │   └── Register.cshtml        # Registration page
│   ├── Video/
│   │   ├── Index.cshtml           # Video library grid
│   │   └── Watch.cshtml           # Video player
│   └── Shared/
│       └── _Layout.cshtml         # Main layout
├── App_Start/
│   └── RouteConfig.cs             # URL routing
├── Web.config                     # Configuration
└── Global.asax.cs                 # Application startup
```

## Setup Instructions

### Prerequisites

1. **Visual Studio 2019/2022** with ASP.NET web development workload
2. **MySQL 8.0** installed and running
3. **.NET Framework 4.8** (included with Visual Studio)

### Step 1: Install MySQL

Download and install MySQL 8.0:
- Windows: https://dev.mysql.com/downloads/installer/
- During installation, set root password (remember this!)

### Step 2: Create Database

Open MySQL Command Line or MySQL Workbench:

```sql
CREATE DATABASE VideoTrackerDB;
```

That's it! Tables will be created automatically when you run the application.

### Step 3: Update Connection String

Open `Web.config` and update the MySQL connection string:

```xml
<connectionStrings>
  <add name="MySqlConnection" 
       connectionString="Server=localhost;Database=VideoTrackerDB;Uid=root;Pwd=YOUR_MYSQL_PASSWORD;" 
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

Replace `YOUR_MYSQL_PASSWORD` with your MySQL root password.

### Step 4: Install NuGet Package

In Visual Studio:
1. Right-click project → Manage NuGet Packages
2. Search for "MySql.Data"
3. Install the package (version 8.0 or higher)

### Step 5: Build and Run

1. Press F5 to build and run
2. Application will:
   - Create database tables automatically
   - Insert 8 sample videos
   - Create demo user account
3. Browser will open to the login page

### Step 6: Login

Use the demo account:
- **Email:** demo@test.com
- **Password:** demo123

Or create your own account by clicking "Register here"

## How It Works

### Simple Login Flow

1. User enters email and password
2. Application checks Users table in MySQL
3. If valid, creates Forms Authentication cookie
4. Stores UserId and Email in Session
5. Redirects to video library

### Progress Tracking Flow

1. User clicks on a video to watch
2. HTML5 video player loads
3. JavaScript tracks current playback time
4. Every minute, sends AJAX request to server
5. Server updates UserVideoProgress table
6. Progress bar updates in real-time

### Database Operations

All database operations use simple ADO.NET:

```csharp
// Example: Get user
using (var conn = DatabaseHelper.GetConnection())
{
    conn.Open();
    string query = "SELECT * FROM Users WHERE Email = @Email";
    using (var cmd = new MySqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Email", email);
        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                // Read user data
            }
        }
    }
}
```

No ORM, no migrations, just simple SQL queries!

## Files Explained

### Controllers

**AccountController.cs**
- `Login()` - Shows login form
- `Login(model)` - Validates user and creates session
- `Register()` - Shows registration form
- `Register(model)` - Creates new user
- `Logout()` - Clears session and redirects to login

**VideoController.cs**
- `Index()` - Shows all videos with user's progress
- `Watch(id)` - Shows video player for specific video
- `UpdateProgress()` - AJAX endpoint to save watch progress

### Data Layer

**DatabaseHelper.cs**
- `GetConnection()` - Returns MySQL connection
- `InitializeDatabase()` - Creates tables and sample data

**UserRepository.cs**
- `GetUserByEmail()` - Find user by email
- `CreateUser()` - Register new user
- `ValidateUser()` - Check email/password

**VideoRepository.cs**
- `GetAllVideos()` - Get all videos
- `GetVideoById()` - Get specific video

**ProgressRepository.cs**
- `GetProgress()` - Get user's progress for a video
- `GetAllProgressForUser()` - Get all progress for user
- `UpdateProgress()` - Save watch progress
- `GetTotalWatchedMinutes()` - Total time watched
- `GetCompletedCount()` - Number of completed videos

### Models

Simple C# classes with properties:
- `User` - UserId, Email, Password, CreatedDate
- `Video` - VideoId, Title, Description, DurationMinutes, Url, ThumbnailUrl
- `UserVideoProgress` - ProgressId, UserId, VideoId, WatchedMinutes, Completed
- `LoginViewModel` - For login/register forms

### Views

Razor views using simple HTML and Bootstrap:
- `Login.cshtml` - Login form with validation
- `Register.cshtml` - Registration form
- `Index.cshtml` - Video grid with progress bars
- `Watch.cshtml` - Video player with progress tracking

## Sample Data

The application includes 8 pre-loaded videos:
1. Introduction to ASP.NET MVC (45 min)
2. MySQL Database Tutorial (60 min)
3. Building Web Applications (90 min)
4. User Authentication (75 min)
5. JavaScript Basics (120 min)
6. HTML & CSS Guide (55 min)
7. Bootstrap Framework (105 min)
8. jQuery Tutorial (50 min)

All videos use free sample videos from Google Cloud Storage.

## Common Issues

### Issue: Cannot connect to MySQL

**Solution:**
1. Make sure MySQL service is running
2. Check connection string has correct password
3. Test connection:
   ```bash
   mysql -u root -p
   ```

### Issue: Tables not created

**Solution:**
- Check MySQL user has CREATE TABLE permissions
- Run application in Debug mode and check output window for errors
- Manually create database: `CREATE DATABASE VideoTrackerDB;`

### Issue: NuGet package error

**Solution:**
- Install MySql.Data package: `Install-Package MySql.Data`
- Restore all NuGet packages: Right-click solution → Restore NuGet Packages

### Issue: Video won't play

**Solution:**
- Check browser console for errors
- Make sure video URL is accessible
- Try different browser (Chrome recommended)

## Configuration

### Change MySQL Connection

Edit `Web.config`:
```xml
<add name="MySqlConnection" 
     connectionString="Server=YOUR_SERVER;Database=YOUR_DB;Uid=YOUR_USER;Pwd=YOUR_PASSWORD;" 
     providerName="MySql.Data.MySqlClient" />
```

### Add More Videos

Edit `DatabaseHelper.cs` → `InsertSampleVideos()` method:
```csharp
"INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
"('Your Title', 'Your Description', 30, 'http://your-video-url.mp4', 'http://thumbnail.jpg')"
```

### Change Session Timeout

Edit `Web.config`:
```xml
<sessionState mode="InProc" timeout="60" />  <!-- 60 minutes -->
```

## Security Notes

⚠️ **This is a simple application for learning purposes:**

- Passwords are stored in plain text (NOT recommended for production)
- No password hashing
- No HTTPS enforcement
- No SQL injection protection beyond parameterized queries
- Basic input validation only

**For production use, you should:**
- Hash passwords (use BCrypt or similar)
- Use HTTPS
- Add more validation
- Implement proper security measures

## Deployment

### Local IIS Deployment

1. Build project in Release mode
2. Publish to folder
3. Create IIS website pointing to published folder
4. Make sure MySQL is accessible from IIS
5. Update connection string for production

### Database Backup

```bash
mysqldump -u root -p VideoTrackerDB > backup.sql
```

### Restore Database

```bash
mysql -u root -p VideoTrackerDB < backup.sql
```

## Future Enhancements

Want to extend this? Here are some ideas:

- [ ] Add password hashing (BCrypt)
- [ ] Add admin panel to upload videos
- [ ] Add video categories
- [ ] Add search functionality
- [ ] Add comments on videos
- [ ] Add user profiles
- [ ] Add social features (share progress)
- [ ] Add certificates for completed courses

## Support

This is a simple educational project. For questions:
1. Check the code comments
2. Review the database schema
3. Check MySQL connection and logs

## License

Free to use for educational purposes.

---

**Built with simple ASP.NET MVC and MySQL - No complex frameworks!**
