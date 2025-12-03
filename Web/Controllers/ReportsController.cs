using System;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;

namespace Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportBLL _reportBLL;

        public ReportsController()
        {
            _reportBLL = new ReportBLL();
        }

        // GET: Reports
        public ActionResult Index(string period = "today")
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);

            // Determine date range based on period
            switch (period.ToLower())
            {
                case "today":
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "week":
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = DateTime.Today.AddDays(1);
                    break;
                case "year":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    endDate = DateTime.Today.AddDays(1);
                    break;
            }

            // Get Year-to-Date Summary
            var ytdSummary = _reportBLL.GetYearToDateSummary();
            ViewBag.YTDRevenue = ytdSummary.TotalRevenue;
            ViewBag.YTDOrders = ytdSummary.TotalOrders;
            ViewBag.TotalProducts = ytdSummary.TotalProducts;

            // Get Daily Sales Report
            var dailySales = _reportBLL.GetDailySalesReport(startDate, endDate);
            ViewBag.DailySales = dailySales;

            // Calculate period summary
            var periodRevenue = dailySales.Sum(x => x.TotalRevenue);
            var periodOrders = dailySales.Sum(x => x.TotalOrders);
            var periodItems = dailySales.Sum(x => x.TotalItemsSold);

            ViewBag.PeriodRevenue = periodRevenue;
            ViewBag.PeriodOrders = periodOrders;
            ViewBag.PeriodItems = periodItems;

            // Get Top Selling Products
            var topProducts = _reportBLL.GetTopSellingProducts(startDate, endDate, 10);
            ViewBag.TopProducts = topProducts;

            // Get Low Stock Products
            var lowStockProducts = _reportBLL.GetLowStockProducts(10);
            ViewBag.LowStockProducts = lowStockProducts;

            // Get Orders by Status
            var ordersByStatus = _reportBLL.GetOrdersByStatusSummary();
            ViewBag.OrdersByStatus = ordersByStatus;

            // Set current period for view
            ViewBag.CurrentPeriod = period;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate.AddDays(-1);

            return View();
        }

        // GET: Reports/Custom
        public ActionResult Custom(DateTime? startDate, DateTime? endDate)
        {
            // Check authentication
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Default to current month if no dates provided
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                endDate = DateTime.Today;
            }

            // Ensure end date is after start date
            if (endDate < startDate)
            {
                TempData["ErrorMessage"] = "End date must be after start date.";
                return RedirectToAction("Index");
            }

            // Get Daily Sales Report
            var dailySales = _reportBLL.GetDailySalesReport(startDate.Value, endDate.Value.AddDays(1));

            // Calculate summary
            var totalRevenue = dailySales.Sum(x => x.TotalRevenue);
            var totalOrders = dailySales.Sum(x => x.TotalOrders);
            var totalItems = dailySales.Sum(x => x.TotalItemsSold);

            ViewBag.StartDate = startDate.Value;
            ViewBag.EndDate = endDate.Value;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalItems = totalItems;
            ViewBag.DailySales = dailySales;

            // Get Top Selling Products for this period
            var topProducts = _reportBLL.GetTopSellingProducts(startDate.Value, endDate.Value.AddDays(1), 10);
            ViewBag.TopProducts = topProducts;

            return View();
        }
    }
}
