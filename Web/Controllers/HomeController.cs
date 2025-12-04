using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderBLL _orderBLL;
        private readonly ProductBLL _productBLL;
        private readonly ReportBLL _reportBLL;

        public HomeController()
        {
            _orderBLL = new OrderBLL();
            _productBLL = new ProductBLL();
            _reportBLL = new ReportBLL();
        }

        public ActionResult Index()
        {
            // Redirect to login if not authenticated
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get dashboard statistics
            var ytdSummary = _reportBLL.GetYearToDateSummary();
            var ordersSummary = _reportBLL.GetOrdersByStatusSummary();
            var lowStockProducts = _productBLL.GetLowStock(10);
            var todaySales = _reportBLL.GetTodaySales();

            // Pass data to view
            ViewBag.TotalRevenue = ytdSummary.TotalRevenue;
            ViewBag.TotalOrders = ytdSummary.TotalOrders;
            ViewBag.TotalProducts = ytdSummary.TotalProducts;
            ViewBag.LowStockCount = lowStockProducts.Count;

            ViewBag.PendingOrders = ordersSummary.ContainsKey("Pending") ? ordersSummary["Pending"] : 0;
            ViewBag.ProcessingOrders = ordersSummary.ContainsKey("Processing") ? ordersSummary["Processing"] : 0;
            ViewBag.ShippedOrders = ordersSummary.ContainsKey("Shipped") ? ordersSummary["Shipped"] : 0;
            ViewBag.DeliveredOrders = ordersSummary.ContainsKey("Delivered") ? ordersSummary["Delivered"] : 0;

            ViewBag.TodayRevenue = todaySales.TotalRevenue;
            ViewBag.TodayOrders = todaySales.TotalOrders;

            ViewBag.CurrentUser = Session["FullName"]?.ToString() ?? "User";
            ViewBag.UserRole = Session["Role"]?.ToString() ?? "Staff";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}