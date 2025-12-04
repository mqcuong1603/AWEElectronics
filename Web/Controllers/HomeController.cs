using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWEElectronics.BLL;
using Web.Filters;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReportBLL _reportBLL;
        private readonly ProductBLL _productBLL;
        private readonly OrderBLL _orderBLL;

        public HomeController()
        {
            _reportBLL = new ReportBLL();
            _productBLL = new ProductBLL();
            _orderBLL = new OrderBLL();
        }

        // Root index - redirect to login or dashboard based on authentication
        public ActionResult Index()
        {
            // If logged in, redirect to dashboard
            if (Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"])
            {
                return RedirectToAction("Dashboard");
            }
            
            // If not logged in, redirect to login page
            return RedirectToAction("Login", "Account");
        }

        [AuthorizeSession]
        public ActionResult Dashboard()
        {
            // Get user role from session FIRST
            string userRole = Session["UserRole"]?.ToString() ?? "Unknown";
            
            // Set default values for ViewBag to prevent errors
            ViewBag.TotalRevenue = 0;
            ViewBag.TotalOrders = 0;
            ViewBag.TotalProducts = 0;
            ViewBag.TodayRevenue = 0;
            ViewBag.TodayOrders = 0;
            ViewBag.OrdersByStatus = new Dictionary<string, int>();
            ViewBag.LowStockCount = 0;
            ViewBag.TopProducts = new List<AWEElectronics.DTO.ProductSalesReport>();
            ViewBag.UserRole = userRole;
            ViewBag.ErrorMessage = null;

            try
            {
                // Try to load dashboard data
                var summary = _reportBLL.GetYearToDateSummary();
                var todaySales = _reportBLL.GetTodaySales();
                var ordersByStatus = _reportBLL.GetOrdersByStatusSummary();
                var lowStock = _productBLL.GetLowStock(10);
                var topProducts = _reportBLL.GetTopSellingProducts(5);

                ViewBag.TotalRevenue = summary.TotalRevenue;
                ViewBag.TotalOrders = summary.TotalOrders;
                ViewBag.TotalProducts = summary.TotalProducts;
                ViewBag.TodayRevenue = todaySales.TotalRevenue;
                ViewBag.TodayOrders = todaySales.TotalOrders;
                ViewBag.OrdersByStatus = ordersByStatus;
                ViewBag.LowStockCount = lowStock.Count;
                ViewBag.TopProducts = topProducts;
            }
            catch (Exception ex)
            {
                // Log error but continue - we'll show the dashboard with default values
                ViewBag.ErrorMessage = "Some dashboard data could not be loaded.";
                System.Diagnostics.Debug.WriteLine($"Dashboard data loading error: {ex.Message}");
            }

            // ALWAYS return role-specific view based on role
            // This happens even if data loading fails above
            string viewName = "Dashboard"; // fallback
            
            switch (userRole?.ToLower())
            {
                case "admin":
                    viewName = "DashboardAdmin";
                    ViewBag.DashboardType = "Admin";
                    ViewBag.CanManageUsers = true;
                    ViewBag.CanManageProducts = true;
                    ViewBag.CanManageOrders = true;
                    ViewBag.CanViewReports = true;
                    ViewBag.CanManageSystem = true;
                    System.Diagnostics.Debug.WriteLine(">>> RENDERING ADMIN DASHBOARD <<<");
                    break;

                case "staff":
                    viewName = "DashboardStaff";
                    ViewBag.DashboardType = "Staff";
                    ViewBag.CanManageUsers = false;
                    ViewBag.CanManageProducts = true;
                    ViewBag.CanManageOrders = true;
                    ViewBag.CanViewReports = true;
                    ViewBag.CanManageSystem = false;
                    
                    // Staff-specific: Pending orders
                    try
                    {
                        var pendingOrders = _orderBLL.GetByStatus("Pending").Take(5).ToList();
                        ViewBag.PendingOrders = pendingOrders;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.PendingOrders = new List<AWEElectronics.DTO.Order>();
                        System.Diagnostics.Debug.WriteLine($"Error loading pending orders: {ex.Message}");
                    }
                    System.Diagnostics.Debug.WriteLine(">>> RENDERING STAFF DASHBOARD <<<");
                    break;

                case "agent":
                    viewName = "DashboardAgent";
                    ViewBag.DashboardType = "Agent";
                    ViewBag.CanManageUsers = false;
                    ViewBag.CanManageProducts = false;
                    ViewBag.CanManageOrders = false;
                    ViewBag.CanViewReports = false;
                    ViewBag.CanManageSystem = false;
                    
                    // Agent-specific: Product counts
                    try
                    {
                        var allProducts = _productBLL.GetAll();
                        ViewBag.TotalProductsCount = allProducts.Count;
                        ViewBag.AvailableProducts = allProducts.Where(p => p.IsPublished).Count();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.TotalProductsCount = 0;
                        ViewBag.AvailableProducts = 0;
                        System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
                    }
                    System.Diagnostics.Debug.WriteLine(">>> RENDERING AGENT DASHBOARD <<<");
                    break;

                case "manager":
                    viewName = "DashboardAdmin";
                    ViewBag.DashboardType = "Manager";
                    ViewBag.CanManageUsers = false;
                    ViewBag.CanManageProducts = true;
                    ViewBag.CanManageOrders = true;
                    ViewBag.CanViewReports = true;
                    ViewBag.CanManageSystem = false;
                    System.Diagnostics.Debug.WriteLine(">>> RENDERING MANAGER DASHBOARD (using Admin view) <<<");
                    break;

                default:
                    viewName = "Dashboard";
                    ViewBag.DashboardType = "Default";
                    System.Diagnostics.Debug.WriteLine($">>> WARNING: Unknown role '{userRole}', using default dashboard <<<");
                    break;
            }

            // Debug output
            System.Diagnostics.Debug.WriteLine($"User Role: {userRole}");
            System.Diagnostics.Debug.WriteLine($"View Name: {viewName}");
            System.Diagnostics.Debug.WriteLine($"Dashboard Type: {ViewBag.DashboardType}");

            // Return the specific view
            return View(viewName);
        }

        [AuthorizeSession]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [AuthorizeSession]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}