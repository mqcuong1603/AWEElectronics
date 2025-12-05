using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Web.Models;

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
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _userBLL.Login(model.Username, model.Password);

            if (result.Success)
            {
                // Create authentication cookie
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                
                // Store user info in session
                Session["UserID"] = result.User.UserID;
                Session["Username"] = result.User.Username;
                Session["FullName"] = result.User.FullName;
                Session["Role"] = result.User.Role;

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                // Redirect based on role
                if (result.User.Role == "Admin" || result.User.Role == "Staff" || result.User.Role == "Accountant")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        // GET: Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                Username = model.Email, // Use email as username
                FullName = model.FullName,
                Email = model.Email,
                Role = "Agent", // Default role for web registration
                Status = "Active"
            };

            var result = _userBLL.CreateUser(user, model.Password);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Profile
        [Authorize]
        public new ActionResult Profile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = (int)Session["UserID"];
            var user = _userBLL.GetById(userId);

            if (user == null)
            {
                return RedirectToAction("Logout");
            }

            return View(user);
        }
    }
}
