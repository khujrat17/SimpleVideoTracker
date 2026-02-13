-- Simple Video Tracker Database Schema
-- Run this script in MySQL to manually create the database and tables

-- Create database
CREATE DATABASE IF NOT EXISTS VideoTrackerDB;
USE VideoTrackerDB;

-- Create Users table
CREATE TABLE IF NOT EXISTS Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create Videos table
CREATE TABLE IF NOT EXISTS Videos (
    VideoId INT PRIMARY KEY AUTO_INCREMENT,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    DurationMinutes INT NOT NULL,
    Url VARCHAR(500) NOT NULL,
    ThumbnailUrl VARCHAR(500),
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Create UserVideoProgress table
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
);

-- Insert demo user
INSERT INTO Users (Email, Password) VALUES ('demo@test.com', 'demo123');

-- Insert sample videos
INSERT INTO Videos (Title, Description, DurationMinutes, Url, ThumbnailUrl) VALUES
('Introduction to ASP.NET MVC', 'Learn the basics of ASP.NET MVC framework', 45, 
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4',
 'https://via.placeholder.com/400x225?text=ASP.NET+MVC'),

('MySQL Database Tutorial', 'Master database operations with MySQL', 60,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4',
 'https://via.placeholder.com/400x225?text=MySQL'),

('Building Web Applications', 'Create web applications step by step', 90,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4',
 'https://via.placeholder.com/400x225?text=Web+Apps'),

('User Authentication', 'Implement login and registration', 75,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4',
 'https://via.placeholder.com/400x225?text=Authentication'),

('JavaScript Basics', 'Learn JavaScript fundamentals', 120,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4',
 'https://via.placeholder.com/400x225?text=JavaScript'),

('HTML & CSS Guide', 'Master HTML and CSS styling', 55,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4',
 'https://via.placeholder.com/400x225?text=HTML+CSS'),

('Bootstrap Framework', 'Build responsive websites with Bootstrap', 105,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerMeltdowns.mp4',
 'https://via.placeholder.com/400x225?text=Bootstrap'),

('jQuery Tutorial', 'Learn jQuery for dynamic web pages', 50,
 'https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4',
 'https://via.placeholder.com/400x225?text=jQuery');

-- Verify data
SELECT 'Users created:' as Info, COUNT(*) as Count FROM Users;
SELECT 'Videos created:' as Info, COUNT(*) as Count FROM Videos;
SELECT 'Demo account email: demo@test.com, password: demo123' as Info;
