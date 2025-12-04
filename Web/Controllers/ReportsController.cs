using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Web.Filters;

namespace Web.Controllers
{
    [AuthorizeSession]
    public class ReportsController : Controller
    {
        private readonly ReportBLL _reportBLL;

        public ReportsController()
        {
            _reportBLL = new ReportBLL();
        }

        // GET: /Reports
        public ActionResult Index(string period, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                List<SalesReport> salesData;
                string reportTitle = "Sales Report";

                // Default to current month if no parameters
                if (string.IsNullOrEmpty(period) && !startDate.HasValue && !endDate.HasValue)
                {
                    period = "month";
                }

                switch (period?.ToLower())
                {
                    case "today":
                        salesData = new List<SalesReport> { _reportBLL.GetTodaySales() };
                        reportTitle = "Today's Sales";
                        break;

                    case "week":
                        salesData = _reportBLL.GetWeeklySales();
                        reportTitle = "This Week's Sales";
                        break;

                    case "month":
                        salesData = _reportBLL.GetMonthlySales();
                        reportTitle = "This Month's Sales";
                        break;

                    case "year":
                        salesData = _reportBLL.GetYearlySales();
                        reportTitle = "This Year's Sales";
                        break;

                    case "custom":
                        if (startDate.HasValue && endDate.HasValue)
                        {
                            salesData = _reportBLL.GetDailySalesReport(startDate.Value, endDate.Value);
                            reportTitle = $"Sales Report ({startDate.Value:MMM dd} - {endDate.Value:MMM dd, yyyy})";
                            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            salesData = _reportBLL.GetMonthlySales();
                            reportTitle = "This Month's Sales";
                            period = "month";
                        }
                        break;

                    default:
                        salesData = _reportBLL.GetMonthlySales();
                        reportTitle = "This Month's Sales";
                        period = "month";
                        break;
                }

                ViewBag.ReportTitle = reportTitle;
                ViewBag.SelectedPeriod = period;

                // Calculate summary
                if (salesData != null && salesData.Any())
                {
                    ViewBag.TotalRevenue = salesData.Sum(s => s.TotalRevenue);
                    ViewBag.TotalOrders = salesData.Sum(s => s.TotalOrders);
                    ViewBag.TotalItems = salesData.Sum(s => s.TotalItemsSold);
                    ViewBag.AverageOrderValue = ViewBag.TotalOrders > 0 ? ViewBag.TotalRevenue / ViewBag.TotalOrders : 0;
                }
                else
                {
                    ViewBag.TotalRevenue = 0;
                    ViewBag.TotalOrders = 0;
                    ViewBag.TotalItems = 0;
                    ViewBag.AverageOrderValue = 0;
                }

                // Get top selling products
                DateTime reportStart = salesData?.FirstOrDefault()?.Date ?? DateTime.Today.AddMonths(-1);
                DateTime reportEnd = salesData?.LastOrDefault()?.Date ?? DateTime.Today;
                var topProducts = _reportBLL.GetTopSellingProducts(reportStart, reportEnd.AddDays(1), 10);
                ViewBag.TopProducts = topProducts;

                // Get orders by status
                var ordersByStatus = _reportBLL.GetOrdersByStatusSummary();
                ViewBag.OrdersByStatus = ordersByStatus;

                return View(salesData ?? new List<SalesReport>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error loading sales report.";
                System.Diagnostics.Debug.WriteLine($"Reports Index error: {ex.Message}");
                return View(new List<SalesReport>());
            }
        }
    }
}
