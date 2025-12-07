using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class OrderDAL
    {
        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            ORDER BY o.OrderDate DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(MapOrder(row));
            }
            return orders;
        }

        public List<Order> GetByStatus(string status)
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            WHERE o.Status = @Status
                            ORDER BY o.OrderDate DESC";

            SqlParameter[] parameters = { new SqlParameter("@Status", status) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(MapOrder(row));
            }
            return orders;
        }

        public List<Order> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
                            ORDER BY o.OrderDate DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(MapOrder(row));
            }
            return orders;
        }

        public List<Order> GetByCustomerId(int customerId)
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            WHERE o.CustomerID = @CustomerID
                            ORDER BY o.OrderDate DESC";

            SqlParameter[] parameters = { new SqlParameter("@CustomerID", customerId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                orders.Add(MapOrder(row));
            }
            return orders;
        }

        public Order GetById(int orderId)
        {
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            WHERE o.OrderID = @OrderID";

            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapOrder(dt.Rows[0]);

            return null;
        }

        public Order GetByOrderCode(string orderCode)
        {
            string query = @"SELECT o.*, c.FullName as CustomerName, c.Email as CustomerEmail,
                            u.FullName as StaffName,
                            CONCAT(a.AddressLine1, ', ', a.City, ', ', a.Country) as ShippingAddress
                            FROM Orders o
                            LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                            LEFT JOIN Users u ON o.StaffCheckedID = u.UserID
                            LEFT JOIN Addresses a ON o.ShippingAddressID = a.AddressID
                            WHERE o.OrderCode = @OrderCode";

            SqlParameter[] parameters = { new SqlParameter("@OrderCode", orderCode) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapOrder(dt.Rows[0]);

            return null;
        }

        public int Insert(Order order)
        {
            string query = @"INSERT INTO Orders (OrderCode, CustomerID, StaffCheckedID, ShippingAddressID, SubTotal, Tax, GrandTotal, Status)
                            VALUES (@OrderCode, @CustomerID, @StaffCheckedID, @ShippingAddressID, @SubTotal, @Tax, @GrandTotal, @Status);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@OrderCode", order.OrderCode),
                new SqlParameter("@CustomerID", order.CustomerID),
                new SqlParameter("@StaffCheckedID", (object)order.StaffCheckedID ?? DBNull.Value),
                new SqlParameter("@ShippingAddressID", order.ShippingAddressID),
                new SqlParameter("@SubTotal", order.SubTotal),
                new SqlParameter("@Tax", order.Tax),
                new SqlParameter("@GrandTotal", order.GrandTotal),
                new SqlParameter("@Status", order.Status ?? "Pending")
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool UpdateStatus(int orderId, string status, int? staffId = null)
        {
            string query = @"UPDATE Orders SET Status = @Status, StaffCheckedID = @StaffID WHERE OrderID = @OrderID";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@Status", status),
                new SqlParameter("@StaffID", (object)staffId ?? DBNull.Value)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int orderId)
        {
            string query = "DELETE FROM Orders WHERE OrderID = @OrderID";
            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public string GenerateOrderCode()
        {
            return $"ORD-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        private Order MapOrder(DataRow row)
        {
            return new Order
            {
                OrderID = Convert.ToInt32(row["OrderID"]),
                OrderCode = row["OrderCode"].ToString(),
                CustomerID = Convert.ToInt32(row["CustomerID"]),
                StaffCheckedID = row["StaffCheckedID"] != DBNull.Value ? Convert.ToInt32(row["StaffCheckedID"]) : (int?)null,
                ShippingAddressID = Convert.ToInt32(row["ShippingAddressID"]),
                SubTotal = Convert.ToDecimal(row["SubTotal"]),
                Tax = Convert.ToDecimal(row["Tax"]),
                GrandTotal = Convert.ToDecimal(row["GrandTotal"]),
                Status = row["Status"].ToString(),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                CustomerName = row.Table.Columns.Contains("CustomerName") ? row["CustomerName"]?.ToString() : null,
                CustomerEmail = row.Table.Columns.Contains("CustomerEmail") ? row["CustomerEmail"]?.ToString() : null,
                StaffName = row.Table.Columns.Contains("StaffName") ? row["StaffName"]?.ToString() : null,
                ShippingAddress = row.Table.Columns.Contains("ShippingAddress") ? row["ShippingAddress"]?.ToString() : null
            };
        }
    }
}
