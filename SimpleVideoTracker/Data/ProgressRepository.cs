using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleVideoTracker.Models;

namespace SimpleVideoTracker.Data
{
    public class ProgressRepository
    {
        public UserVideoProgress GetProgress(int userId, int videoId)
        {
            UserVideoProgress progress = null;
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProgressId, UserId, VideoId, WatchedMinutes, Completed, LastWatchedDate " +
                              "FROM UserVideoProgress WHERE UserId = @UserId AND VideoId = @VideoId";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@VideoId", videoId);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            progress = new UserVideoProgress
                            {
                                ProgressId = reader.GetInt32("ProgressId"),
                                UserId = reader.GetInt32("UserId"),
                                VideoId = reader.GetInt32("VideoId"),
                                WatchedMinutes = reader.GetInt32("WatchedMinutes"),
                                Completed = reader.GetBoolean("Completed"),
                                LastWatchedDate = reader.GetDateTime("LastWatchedDate")
                            };
                        }
                    }
                }
            }
            
            return progress;
        }

        public Dictionary<int, UserVideoProgress> GetAllProgressForUser(int userId)
        {
            var progressDict = new Dictionary<int, UserVideoProgress>();
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT ProgressId, UserId, VideoId, WatchedMinutes, Completed, LastWatchedDate " +
                              "FROM UserVideoProgress WHERE UserId = @UserId";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int videoId = reader.GetInt32("VideoId");
                            progressDict[videoId] = new UserVideoProgress
                            {
                                ProgressId = reader.GetInt32("ProgressId"),
                                UserId = reader.GetInt32("UserId"),
                                VideoId = videoId,
                                WatchedMinutes = reader.GetInt32("WatchedMinutes"),
                                Completed = reader.GetBoolean("Completed"),
                                LastWatchedDate = reader.GetDateTime("LastWatchedDate")
                            };
                        }
                    }
                }
            }
            
            return progressDict;
        }

        public bool UpdateProgress(int userId, int videoId, int watchedMinutes, bool completed)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                
                // Check if progress exists
                string checkQuery = "SELECT COUNT(*) FROM UserVideoProgress WHERE UserId = @UserId AND VideoId = @VideoId";
                using (var checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@UserId", userId);
                    checkCmd.Parameters.AddWithValue("@VideoId", videoId);
                    
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    
                    if (count > 0)
                    {
                        // Update existing progress
                        string updateQuery = "UPDATE UserVideoProgress SET WatchedMinutes = @WatchedMinutes, " +
                                           "Completed = @Completed, LastWatchedDate = NOW() " +
                                           "WHERE UserId = @UserId AND VideoId = @VideoId";
                        
                        using (var updateCmd = new MySqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@WatchedMinutes", watchedMinutes);
                            updateCmd.Parameters.AddWithValue("@Completed", completed);
                            updateCmd.Parameters.AddWithValue("@UserId", userId);
                            updateCmd.Parameters.AddWithValue("@VideoId", videoId);
                            
                            return updateCmd.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        // Insert new progress
                        string insertQuery = "INSERT INTO UserVideoProgress (UserId, VideoId, WatchedMinutes, Completed) " +
                                           "VALUES (@UserId, @VideoId, @WatchedMinutes, @Completed)";
                        
                        using (var insertCmd = new MySqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@UserId", userId);
                            insertCmd.Parameters.AddWithValue("@VideoId", videoId);
                            insertCmd.Parameters.AddWithValue("@WatchedMinutes", watchedMinutes);
                            insertCmd.Parameters.AddWithValue("@Completed", completed);
                            
                            return insertCmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
        }

        public int GetTotalWatchedMinutes(int userId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT IFNULL(SUM(WatchedMinutes), 0) FROM UserVideoProgress WHERE UserId = @UserId";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetCompletedCount(int userId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM UserVideoProgress WHERE UserId = @UserId AND Completed = TRUE";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
