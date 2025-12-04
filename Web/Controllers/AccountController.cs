using System;
using System.Web;
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

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // If already logged in, redirect to dashboard
            if (Session["User"] != null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password are required.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Authenticate using BLL
            LoginResult result = _userBLL.Login(username, password);

            if (result.Success)
            {
                // Store user in session
                Session["User"] = result.User;
                Session["UserID"] = result.User.UserID;
                Session["Username"] = result.User.Username;
                Session["FullName"] = result.User.FullName;
                Session["Role"] = result.User.Role;

                // Redirect to return URL or dashboard
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            // Clear session
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        // Helper method to check if user is authenticated
        protected bool IsAuthenticated()
        {
            return Session["User"] != null;
        }

        // Helper method to get current user
        protected User GetCurrentUser()
        {
            return Session["User"] as User;
        }
    }
}
