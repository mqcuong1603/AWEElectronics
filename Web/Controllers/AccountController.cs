using System;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserBLL _userBLL;

        public AccountController()
        {
            _userBLL = new UserBLL();
        }

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            // If already logged in, redirect to appropriate dashboard based on role
            if (Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"])
            {
                return RedirectToDashboardByRole();
            }

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }

            try
            {
                LoginResult result = _userBLL.Login(username, password);

                if (result.Success)
                {
                    // Set session variables
                    Session["IsLoggedIn"] = true;
                    Session["UserId"] = result.User.UserID;
                    Session["Username"] = result.User.Username;
                    Session["FullName"] = result.User.FullName;
                    Session["UserRole"] = result.User.Role;
                    Session["Email"] = result.User.Email;

                    // Redirect based on user role
                    return RedirectToDashboardByRole();
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                // In debug mode, show the actual error for troubleshooting
                #if DEBUG
                ViewBag.ErrorMessage = $"An error occurred during login: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}\nStack trace: {ex.StackTrace}");
                #else
                ViewBag.ErrorMessage = "An error occurred during login. Please try again.";
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                #endif
                return View();
            }
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }

        // GET: /Account/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Account/Profile
        public ActionResult Profile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = (int)Session["UserId"];
            User user = _userBLL.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("Logout");
            }

            return View(user);
        }

        // Helper method to redirect based on user role
        private ActionResult RedirectToDashboardByRole()
        {
            string userRole = Session["UserRole"] as string;

            // You can customize landing pages based on roles
            switch (userRole?.ToLower())
            {
                case "admin":
                    // Admin goes to dashboard
                    return RedirectToAction("Dashboard", "Home");

                case "staff":
                    // Staff goes to dashboard (can be customized)
                    return RedirectToAction("Dashboard", "Home");

                case "agent":
                    // Agent goes to dashboard (can be customized)
                    return RedirectToAction("Dashboard", "Home");

                default:
                    // Default to dashboard
                    return RedirectToAction("Dashboard", "Home");
            }
        }
    }
}
