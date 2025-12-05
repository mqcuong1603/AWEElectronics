using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserBLL _userBLL;
        private readonly ProductBLL _productBLL;
        private readonly OrderBLL _orderBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly SupplierBLL _supplierBLL;

        public AdminController()
        {
            _userBLL = new UserBLL();
            _productBLL = new ProductBLL();
            _orderBLL = new OrderBLL();
            _categoryBLL = new CategoryBLL();
            _supplierBLL = new SupplierBLL();
        }

        // GET: Admin - Route to role-specific dashboard
        public ActionResult Index()
        {
            // Check if user has any admin role
            if (Session["Role"] == null)
            {
                TempData["ErrorMessage"] = "Access denied. Please login.";
                return RedirectToAction("Login", "Account");
            }

            string role = Session["Role"].ToString();

            // Route to appropriate dashboard based on role
            switch (role)
            {
                case "Admin":
                    return AdminDashboard();
                case "Staff":
                    return StaffDashboard();
                case "Accountant":
                    return AccountantDashboard();
                default:
                    TempData["ErrorMessage"] = "Access denied. You don't have permission to access this area.";
                    return RedirectToAction("Index", "Home");
            }
        }

        // Admin Dashboard
        private ActionResult AdminDashboard()
        {
            ViewBag.UserName = Session["FullName"];
            ViewBag.Role = Session["Role"];

            // Get statistics
            var users = _userBLL.GetAll();
            var products = _productBLL.GetAll();
            var orders = _orderBLL.GetAll();

            ViewBag.TotalUsers = users.Count;
            ViewBag.ActiveUsers = users.Count(u => u.Status == "Active");
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalOrders = orders.Count;
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");
            ViewBag.LowStockProducts = products.Count(p => p.StockLevel <= 10);

            return View("AdminDashboard");
        }

        // Staff Dashboard
        private ActionResult StaffDashboard()
        {
            ViewBag.UserName = Session["FullName"];
            ViewBag.Role = Session["Role"];

            // Get statistics relevant to staff
            var products = _productBLL.GetAll();
            var orders = _orderBLL.GetAll();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalOrders = orders.Count;
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");
            ViewBag.ProcessingOrders = orders.Count(o => o.Status == "Processing");
            ViewBag.LowStockProducts = products.Count(p => p.StockLevel <= 10);
            ViewBag.OutOfStockProducts = products.Count(p => p.StockLevel == 0);

            return View("StaffDashboard");
        }

        // Accountant Dashboard
        private ActionResult AccountantDashboard()
        {
            ViewBag.UserName = Session["FullName"];
            ViewBag.Role = Session["Role"];

            // Get financial statistics
            var orders = _orderBLL.GetAll();

            ViewBag.TotalOrders = orders.Count;
            ViewBag.CompletedOrders = orders.Count(o => o.Status == "Completed");
            ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");
            ViewBag.TotalRevenue = orders.Where(o => o.Status == "Completed").Sum(o => o.GrandTotal);
            ViewBag.PendingRevenue = orders.Where(o => o.Status == "Pending" || o.Status == "Processing").Sum(o => o.GrandTotal);

            return View("AccountantDashboard");
        }

        #region User Management
        // GET: Admin/Users
        public ActionResult Users()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var users = _userBLL.GetAll();
            return View(users);
        }

        // GET: Admin/CreateUser
        public ActionResult CreateUser()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User user, string password)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Password is required.");
                return View(user);
            }

            var result = _userBLL.CreateUser(user, password);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "User created successfully!";
                return RedirectToAction("Users");
            }

            ModelState.AddModelError("", result.Message);
            return View(user);
        }

        // GET: Admin/EditUser/{id}
        public ActionResult EditUser(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var user = _userBLL.GetById(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Users");
            }

            return View(user);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                TempData["ErrorMessage"] = "Access denied. Admin privileges required.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var result = _userBLL.UpdateUser(user);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "User updated successfully!";
                return RedirectToAction("Users");
            }

            ModelState.AddModelError("", result.Message);
            return View(user);
        }

        // POST: Admin/DeleteUser/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var result = _userBLL.DeleteUser(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Users");
        }
        #endregion

        #region Product Management
        // GET: Admin/Products
        public ActionResult Products()
        {
            // Allow Admin and Staff
            if (Session["Role"] == null)
            {
                TempData["ErrorMessage"] = "Access denied. Please login.";
                return RedirectToAction("Login", "Account");
            }

            string role = Session["Role"].ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Admin or Staff privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var products = _productBLL.GetAll();
            return View(products);
        }

        // GET: Admin/CreateProduct
        public ActionResult CreateProduct()
        {
            // Allow Admin and Staff
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Admin or Staff privileges required.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.Suppliers = _supplierBLL.GetAll();
            return View();
        }

        // POST: Admin/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Product product)
        {
            // Allow Admin and Staff
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Admin or Staff privileges required.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryBLL.GetAll();
                ViewBag.Suppliers = _supplierBLL.GetAll();
                return View(product);
            }

            var result = _productBLL.CreateProduct(product);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Products");
            }

            ModelState.AddModelError("", result.Message);
            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.Suppliers = _supplierBLL.GetAll();
            return View(product);
        }

        // GET: Admin/EditProduct/{id}
        public ActionResult EditProduct(int id)
        {
            // Allow Admin and Staff
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Admin or Staff privileges required.";
                return RedirectToAction("Index", "Home");
            }

            var product = _productBLL.GetById(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Products");
            }

            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.Suppliers = _supplierBLL.GetAll();
            return View(product);
        }

        // POST: Admin/EditProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(Product product)
        {
            // Allow Admin and Staff
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Admin or Staff privileges required.";
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryBLL.GetAll();
                ViewBag.Suppliers = _supplierBLL.GetAll();
                return View(product);
            }

            var result = _productBLL.UpdateProduct(product);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Products");
            }

            ModelState.AddModelError("", result.Message);
            ViewBag.Categories = _categoryBLL.GetAll();
            ViewBag.Suppliers = _supplierBLL.GetAll();
            return View(product);
        }

        // POST: Admin/DeleteProduct/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return Json(new { success = false, message = "Access denied" });
            }

            var result = _productBLL.DeleteProduct(id);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Products");
        }
        #endregion

        #region Order Management
        // GET: Admin/Orders
        public ActionResult Orders()
        {
            // Allow Admin, Staff, and Accountant
            if (Session["Role"] == null)
            {
                TempData["ErrorMessage"] = "Access denied. Please login.";
                return RedirectToAction("Login", "Account");
            }

            string role = Session["Role"].ToString();
            if (role != "Admin" && role != "Staff" && role != "Accountant")
            {
                TempData["ErrorMessage"] = "Access denied. Insufficient privileges.";
                return RedirectToAction("Index", "Home");
            }

            var orders = _orderBLL.GetAll();
            return View(orders);
        }

        // GET: Admin/OrderDetails/{id}
        public ActionResult OrderDetails(int id)
        {
            // Allow Admin, Staff, and Accountant
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff" && role != "Accountant")
            {
                TempData["ErrorMessage"] = "Access denied. Insufficient privileges.";
                return RedirectToAction("Index", "Home");
            }

            var order = _orderBLL.GetById(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Orders");
            }

            return View(order);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderStatus(int orderId, string newStatus)
        {
            // Allow Admin and Staff only (not Accountant)
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Only Admin or Staff can update order status.";
                return RedirectToAction("OrderDetails", new { id = orderId });
            }

            int staffId = (int)Session["UserID"];
            var result = _orderBLL.UpdateOrderStatus(orderId, newStatus, staffId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("OrderDetails", new { id = orderId });
        }

        // POST: Admin/CancelOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelOrder(int orderId)
        {
            // Allow Admin and Staff only
            string role = Session["Role"]?.ToString();
            if (role != "Admin" && role != "Staff")
            {
                TempData["ErrorMessage"] = "Access denied. Only Admin or Staff can cancel orders.";
                return RedirectToAction("Orders");
            }

            int staffId = (int)Session["UserID"];
            var result = _orderBLL.CancelOrder(orderId, staffId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction("Orders");
        }
        #endregion
    }
}
