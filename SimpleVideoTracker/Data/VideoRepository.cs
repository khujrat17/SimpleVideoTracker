using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleVideoTracker.Models;

namespace SimpleVideoTracker.Data
{
    public class VideoRepository
    {
        public List<Video> GetAllVideos()
        {
            var videos = new List<Video>();
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT VideoId, Title, Description, DurationMinutes, Url, ThumbnailUrl, CreatedDate FROM Videos ORDER BY CreatedDate DESC";
                
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        videos.Add(new Video
                        {
                            VideoId = reader.GetInt32("VideoId"),
                            Title = reader.GetString("Title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                            DurationMinutes = reader.GetInt32("DurationMinutes"),
                            Url = reader.GetString("Url"),
                            ThumbnailUrl = reader.IsDBNull(reader.GetOrdinal("ThumbnailUrl")) ? "" : reader.GetString("ThumbnailUrl"),
                            CreatedDate = reader.GetDateTime("CreatedDate")
                        });
                    }
                }
            }
            
            return videos;
        }

        public Video GetVideoById(int videoId)
        {
            Video video = null;
            
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT VideoId, Title, Description, DurationMinutes, Url, ThumbnailUrl, CreatedDate FROM Videos WHERE VideoId = @VideoId";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@VideoId", videoId);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            video = new Video
                            {
                                VideoId = reader.GetInt32("VideoId"),
                                Title = reader.GetString("Title"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                                DurationMinutes = reader.GetInt32("DurationMinutes"),
                                Url = reader.GetString("Url"),
                                ThumbnailUrl = reader.IsDBNull(reader.GetOrdinal("ThumbnailUrl")) ? "" : reader.GetString("ThumbnailUrl"),
                                CreatedDate = reader.GetDateTime("CreatedDate")
                            };
                        }
                    }
                }
            }
            
            return video;
        }
    }
}
