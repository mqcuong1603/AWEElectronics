using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class ReportDAL
    {
        public List<SalesReport> GetDailySales(DateTime startDate, DateTime endDate)
        {
            List<SalesReport> reports = new List<SalesReport>();
            string query = @"SELECT
                            CAST(o.OrderDate AS DATE) as Date,
                            COUNT(DISTINCT o.OrderID) as TotalOrders,
                            SUM(o.GrandTotal) as TotalRevenue,
                            SUM(od.Quantity) as TotalItemsSold
                            FROM Orders o
                            LEFT JOIN OrderDetails od ON o.OrderID = od.OrderID
                            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                            AND o.Status != 'Cancelled'
                            GROUP BY CAST(o.OrderDate AS DATE)
                            ORDER BY Date";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                reports.Add(new SalesReport
                {
                    Date = Convert.ToDateTime(row["Date"]),
                    TotalOrders = Convert.ToInt32(row["TotalOrders"]),
                    TotalRevenue = Convert.ToDecimal(row["TotalRevenue"]),
                    TotalItemsSold = row["TotalItemsSold"] != DBNull.Value ? Convert.ToInt32(row["TotalItemsSold"]) : 0
                });
            }
            return reports;
        }

        public List<ProductSalesReport> GetTopSellingProducts(DateTime startDate, DateTime endDate, int top = 10)
        {
            List<ProductSalesReport> reports = new List<ProductSalesReport>();
            string query = @"SELECT TOP (@Top)
                            p.ProductID,
                            p.Name as ProductName,
                            p.SKU,
                            SUM(od.Quantity) as QuantitySold,
                            SUM(od.Total) as TotalRevenue
                            FROM OrderDetails od
                            INNER JOIN Orders o ON od.OrderID = o.OrderID
                            INNER JOIN Products p ON od.ProductID = p.ProductID
                            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                            AND o.Status != 'Cancelled'
                            GROUP BY p.ProductID, p.Name, p.SKU
                            ORDER BY QuantitySold DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@Top", top),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                reports.Add(new ProductSalesReport
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    SKU = row["SKU"].ToString(),
                    QuantitySold = Convert.ToInt32(row["QuantitySold"]),
                    TotalRevenue = Convert.ToDecimal(row["TotalRevenue"])
                });
            }
            return reports;
        }

        public decimal GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            string query = @"SELECT ISNULL(SUM(GrandTotal), 0)
                            FROM Orders
                            WHERE OrderDate BETWEEN @StartDate AND @EndDate
                            AND Status != 'Cancelled'";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public int GetTotalOrders(DateTime startDate, DateTime endDate)
        {
            string query = @"SELECT COUNT(*)
                            FROM Orders
                            WHERE OrderDate BETWEEN @StartDate AND @EndDate
                            AND Status != 'Cancelled'";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}
