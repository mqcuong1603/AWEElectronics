using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Web.Filters;

namespace Web.Controllers
{
    [AuthorizeSession]
    [AuthorizeRole("Admin")]
    public class UsersController : Controller
    {
        private readonly UserBLL _userBLL;

        public UsersController()
        {
            _userBLL = new UserBLL();
        }

        // GET: /Users
        public ActionResult Index(string role, string status)
        {
            try
            {
                List<User> users;

                if (!string.IsNullOrWhiteSpace(role))
                {
                    users = _userBLL.GetByRole(role);
                    ViewBag.SelectedRole = role;
                }
                else
                {
                    users = _userBLL.GetAll();
                }

                ViewBag.SelectedStatus = status;

                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading users.";
                System.Diagnostics.Debug.WriteLine($"Users Index error: {ex.Message}");
                return View(new List<User>());
            }
        }

        // GET: /Users/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                User user = _userBLL.GetById(id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = "User not found.";
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading user details.";
                System.Diagnostics.Debug.WriteLine($"User Details error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // GET: /Users/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new List<string> { "Admin", "Staff", "Accountant", "Agent" };
            ViewBag.Statuses = new List<string> { "Active", "Inactive", "Locked" };
            return View();
        }

        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, string password, string confirmPassword)
        {
            try
            {
                ViewBag.Roles = new List<string> { "Admin", "Staff", "Accountant", "Agent" };
                ViewBag.Statuses = new List<string> { "Active", "Inactive", "Locked" };

                if (string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.ErrorMessage = "Password is required.";
                    return View(user);
                }

                if (password != confirmPassword)
                {
                    ViewBag.ErrorMessage = "Passwords do not match.";
                    return View(user);
                }

                var result = _userBLL.CreateUser(user, password);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error creating user: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"User Create error: {ex.Message}");
                return View(user);
            }
        }

        // GET: /Users/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                User user = _userBLL.GetById(id);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.Roles = new List<string> { "Admin", "Staff", "Accountant", "Agent" };
                ViewBag.Statuses = new List<string> { "Active", "Inactive", "Locked" };

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading user for editing.";
                System.Diagnostics.Debug.WriteLine($"User Edit GET error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                ViewBag.Roles = new List<string> { "Admin", "Staff", "Accountant", "Agent" };
                ViewBag.Statuses = new List<string> { "Active", "Inactive", "Locked" };

                var result = _userBLL.UpdateUser(user);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error updating user: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"User Edit POST error: {ex.Message}");
                return View(user);
            }
        }

        // POST: /Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                // Prevent deleting the current logged-in user
                int currentUserId = Session["UserId"] != null ? (int)Session["UserId"] : 0;
                if (id == currentUserId)
                {
                    TempData["ErrorMessage"] = "You cannot delete your own account.";
                    return RedirectToAction("Index");
                }

                var result = _userBLL.DeleteUser(id);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting user: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"User Delete error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // GET: /Users/ChangePassword/5
        public ActionResult ChangePassword(int id)
        {
            try
            {
                User user = _userBLL.GetById(id);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.User = user;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading change password page.";
                System.Diagnostics.Debug.WriteLine($"ChangePassword GET error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // POST: /Users/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int id, string currentPassword, string newPassword, string confirmPassword)
        {
            try
            {
                User user = _userBLL.GetById(id);
                ViewBag.User = user;

                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    ViewBag.ErrorMessage = "New password is required.";
                    return View();
                }

                if (newPassword != confirmPassword)
                {
                    ViewBag.ErrorMessage = "New passwords do not match.";
                    return View();
                }

                var result = _userBLL.ChangePassword(id, currentPassword, newPassword);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error changing password: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ChangePassword POST error: {ex.Message}");
                return View();
            }
        }
    }
}
