using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace SimpleVideoTracker.Data
{
    public class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void InitializeDatabase()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                
                // Create Users table
                string createUsersTable = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        UserId INT PRIMARY KEY AUTO_INCREMENT,
                        Email VARCHAR(255) NOT NULL UNIQUE,
                        Password VARCHAR(255) NOT NULL,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                
                using (var cmd = new MySqlCommand(createUsersTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Create Videos table
                string createVideosTable = @"
                    CREATE TABLE IF NOT EXISTS Videos (
                        VideoId INT PRIMARY KEY AUTO_INCREMENT,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT,
                        DurationMinutes INT NOT NULL,
                        Url VARCHAR(500) NOT NULL,
                        ThumbnailUrl VARCHAR(500),
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                
                using (var cmd = new MySqlCommand(createVideosTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Create UserVideoProgress table
                string createProgressTable = @"
                    CREATE TABLE IF NOT EXISTS UserVideoProgress (
                        ProgressId INT PRIMARY KEY AUTO_INCREMENT,
                        UserId INT NOT NULL,
                        VideoId INT NOT NULL,
                        WatchedMinutes INT DEFAULT 0,
                        Completed BOOLEAN DEFAULT FALSE,
                        LastWatchedDate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        UNIQUE KEY unique_user_video (UserId, VideoId),
                        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
                        FOREIGN KEY (VideoId) REFERENCES Videos(VideoId) ON DELETE CASCADE
                    )";
                
                using (var cmd = new MySqlCommand(createProgressTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Insert sample videos if table is empty
                string checkVideos = "SELECT COUNT(*) FROM Videos";
                using (var cmd = new MySqlCommand(checkVideos, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        InsertSampleVideos(conn);
                    }
                }

                // Insert demo user if not exists
                string checkUser = "SELECT COUNT(*) FROM Users WHERE Email = 'demo@test.com'";
                using (var cmd = new MySqlCommand(checkUser, conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        string insertUser = "INSERT INTO Users (Email, Password) VALUES ('demo@test.com', 'demo123')";
                        using (var insertCmd = new MySqlCommand(insertUser, conn))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void InsertSampleVideos(MySqlConnection conn)
        {
            string[] videos = new string[]
            {
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('Introduction to ASP.NET MVC', 'Learn the basics of ASP.NET MVC framework', 45, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4', 'https://via.placeholder.com/400x225?text=ASP.NET+MVC')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('MySQL Database Tutorial', 'Master database operations with MySQL', 60, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4', 'https://via.placeholder.com/400x225?text=MySQL')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('Building Web Applications', 'Create web applications step by step', 90, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4', 'https://via.placeholder.com/400x225?text=Web+Apps')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('User Authentication', 'Implement login and registration', 75, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4', 'https://via.placeholder.com/400x225?text=Authentication')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('JavaScript Basics', 'Learn JavaScript fundamentals', 120, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4', 'https://via.placeholder.com/400x225?text=JavaScript')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('HTML & CSS Guide', 'Master HTML and CSS styling', 55, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4', 'https://via.placeholder.com/400x225?text=HTML+CSS')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('Bootstrap Framework', 'Build responsive websites with Bootstrap', 105, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerMeltdowns.mp4', 'https://via.placeholder.com/400x225?text=Bootstrap')",
                
                "INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES " +
                "('jQuery Tutorial', 'Learn jQuery for dynamic web pages', 50, 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4', 'https://via.placeholder.com/400x225?text=jQuery')"
            };

            foreach (string sql in videos)
            {
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
