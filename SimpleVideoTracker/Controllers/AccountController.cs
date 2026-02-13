using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SimpleVideoTracker.Data;
using SimpleVideoTracker.Models;

namespace SimpleVideoTracker.Controllers
{
    public class AccountController : Controller
    {
        private UserRepository userRepo = new UserRepository();

        // GET: Account/Login
        public ActionResult Login()
        {
            // Redirect if already logged in
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Video");
            }
            
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Simple validation
            if (userRepo.ValidateUser(model.Email, model.Password))
            {
                // Get user details
                var user = userRepo.GetUserByEmail(model.Email);
                
                // Create authentication ticket
                FormsAuthentication.SetAuthCookie(user.UserId.ToString(), model.RememberMe);
                
                // Store email in session
                Session["UserEmail"] = user.Email;
                Session["UserId"] = user.UserId;
                
                return RedirectToAction("Index", "Video");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Video");
            }
            
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if user already exists
            var existingUser = userRepo.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already registered");
                return View(model);
            }

            // Create new user
            if (userRepo.CreateUser(model.Email, model.Password))
            {
                // Auto login after registration
                var user = userRepo.GetUserByEmail(model.Email);
                FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                Session["UserEmail"] = user.Email;
                Session["UserId"] = user.UserId;
                
                return RedirectToAction("Index", "Video");
            }

            ModelState.AddModelError("", "Registration failed. Please try again.");
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            
            return RedirectToAction("Login");
        }
    }
}
