using System;
using System.Web.Mvc;
using SimpleVideoTracker.Data;

namespace SimpleVideoTracker.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        private VideoRepository videoRepo = new VideoRepository();
        private ProgressRepository progressRepo = new ProgressRepository();

        // GET: Video/Index
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            
            // Get all videos
            var videos = videoRepo.GetAllVideos();
            
            // Get user progress for all videos
            var progressDict = progressRepo.GetAllProgressForUser(userId);
            
            // Get statistics
            ViewBag.TotalVideos = videos.Count;
            ViewBag.CompletedCount = progressRepo.GetCompletedCount(userId);
            ViewBag.TotalWatchedMinutes = progressRepo.GetTotalWatchedMinutes(userId);
            ViewBag.TotalWatchedHours = Math.Round(ViewBag.TotalWatchedMinutes / 60.0, 1);
            ViewBag.UserEmail = Session["UserEmail"];
            ViewBag.ProgressDict = progressDict;
            
            return View(videos);
        }

        // GET: Video/Watch/5
        public ActionResult Watch(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            
            var video = videoRepo.GetVideoById(id);
            if (video == null)
            {
                return HttpNotFound();
            }

            var progress = progressRepo.GetProgress(userId, id);
            
            ViewBag.Progress = progress;
            ViewBag.UserId = userId;
            
            return View(video);
        }

        // POST: Video/UpdateProgress
        [HttpPost]
        public JsonResult UpdateProgress(int videoId, int watchedMinutes)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return Json(new { success = false, message = "Not logged in" });
                }

                int userId = Convert.ToInt32(Session["UserId"]);
                
                // Get video to check duration
                var video = videoRepo.GetVideoById(videoId);
                if (video == null)
                {
                    return Json(new { success = false, message = "Video not found" });
                }

                // Determine if completed
                bool completed = watchedMinutes >= video.DurationMinutes;
                
                // Update progress
                bool success = progressRepo.UpdateProgress(userId, videoId, watchedMinutes, completed);
                
                if (success)
                {
                    double percentage = video.DurationMinutes > 0 
                        ? Math.Min((double)watchedMinutes / video.DurationMinutes * 100, 100) 
                        : 0;
                    
                    return Json(new 
                    { 
                        success = true,
                        watchedMinutes = watchedMinutes,
                        completed = completed,
                        percentage = Math.Round(percentage, 1)
                    });
                }

                return Json(new { success = false, message = "Failed to update progress" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
