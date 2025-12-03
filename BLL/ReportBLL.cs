using System;
using System.Collections.Generic;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

namespace AWEElectronics.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _reportDAL;
        private readonly OrderDAL _orderDAL;
        private readonly ProductDAL _productDAL;

        public ReportBLL()
        {
            _reportDAL = new ReportDAL();
            _orderDAL = new OrderDAL();
            _productDAL = new ProductDAL();
        }

        // Daily sales report
        public List<SalesReport> GetDailySalesReport(DateTime startDate, DateTime endDate)
        {
            return _reportDAL.GetDailySales(startDate, endDate);
        }

        // Today's sales
        public SalesReport GetTodaySales()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            var reports = _reportDAL.GetDailySales(today, tomorrow);
            return reports.Count > 0 ? reports[0] : new SalesReport { Date = today };
        }

        // This week's sales
        public List<SalesReport> GetWeeklySales()
        {
            DateTime today = DateTime.Today;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            return _reportDAL.GetDailySales(startOfWeek, today.AddDays(1));
        }

        // This month's sales
        public List<SalesReport> GetMonthlySales()
        {
            DateTime today = DateTime.Today;
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            return _reportDAL.GetDailySales(startOfMonth, today.AddDays(1));
        }

        // This year's sales
        public List<SalesReport> GetYearlySales()
        {
            DateTime today = DateTime.Today;
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            return _reportDAL.GetDailySales(startOfYear, today.AddDays(1));
        }

        // Year to date summary
        public (decimal TotalRevenue, int TotalOrders, int TotalProducts) GetYearToDateSummary()
        {
            DateTime today = DateTime.Today;
            DateTime startOfYear = new DateTime(today.Year, 1, 1);

            decimal revenue = _reportDAL.GetTotalRevenue(startOfYear, today.AddDays(1));
            int orders = _reportDAL.GetTotalOrders(startOfYear, today.AddDays(1));
            int products = _productDAL.GetAll().Count;

            return (revenue, orders, products);
        }

        // Top selling products
        public List<ProductSalesReport> GetTopSellingProducts(int top = 10)
        {
            DateTime today = DateTime.Today;
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            return _reportDAL.GetTopSellingProducts(startOfYear, today.AddDays(1), top);
        }

        public List<ProductSalesReport> GetTopSellingProducts(DateTime startDate, DateTime endDate, int top = 10)
        {
            return _reportDAL.GetTopSellingProducts(startDate, endDate, top);
        }

        // Low stock products
        public List<Product> GetLowStockProducts(int threshold = 10)
        {
            return _productDAL.GetLowStock(threshold);
        }

        // Orders by status summary
        public Dictionary<string, int> GetOrdersByStatusSummary()
        {
            var summary = new Dictionary<string, int>();
            string[] statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

            foreach (var status in statuses)
            {
                var orders = _orderDAL.GetByStatus(status);
                summary[status] = orders.Count;
            }

            return summary;
        }

        // Custom date range report
        public (decimal TotalRevenue, int TotalOrders) GetSalesSummary(DateTime startDate, DateTime endDate)
        {
            decimal revenue = _reportDAL.GetTotalRevenue(startDate, endDate);
            int orders = _reportDAL.GetTotalOrders(startDate, endDate);
            return (revenue, orders);
        }
    }
}
