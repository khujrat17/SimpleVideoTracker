using System;

namespace SimpleVideoTracker.Models
{
    public class UserVideoProgress
    {
        public int ProgressId { get; set; }
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public int WatchedMinutes { get; set; }
        public bool Completed { get; set; }
        public DateTime LastWatchedDate { get; set; }
    }
}
