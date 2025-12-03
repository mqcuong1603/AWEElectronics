using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class OrderDetailDAL
    {
        public List<OrderDetail> GetByOrderId(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();
            string query = @"SELECT od.*, p.Name as ProductName, p.SKU as ProductSKU
                            FROM OrderDetails od
                            LEFT JOIN Products p ON od.ProductID = p.ProductID
                            WHERE od.OrderID = @OrderID";

            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                details.Add(MapOrderDetail(row));
            }
            return details;
        }

        public int Insert(OrderDetail detail)
        {
            string query = @"INSERT INTO OrderDetails (OrderID, ProductID, UnitPrice, Quantity, Total)
                            VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity, @Total);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", detail.OrderID),
                new SqlParameter("@ProductID", detail.ProductID),
                new SqlParameter("@UnitPrice", detail.UnitPrice),
                new SqlParameter("@Quantity", detail.Quantity),
                new SqlParameter("@Total", detail.Total)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Delete(int detailId)
        {
            string query = "DELETE FROM OrderDetails WHERE DetailID = @DetailID";
            SqlParameter[] parameters = { new SqlParameter("@DetailID", detailId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteByOrderId(int orderId)
        {
            string query = "DELETE FROM OrderDetails WHERE OrderID = @OrderID";
            SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private OrderDetail MapOrderDetail(DataRow row)
        {
            return new OrderDetail
            {
                DetailID = Convert.ToInt32(row["DetailID"]),
                OrderID = Convert.ToInt32(row["OrderID"]),
                ProductID = Convert.ToInt32(row["ProductID"]),
                UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                Quantity = Convert.ToInt32(row["Quantity"]),
                Total = Convert.ToDecimal(row["Total"]),
                ProductName = row.Table.Columns.Contains("ProductName") ? row["ProductName"]?.ToString() : null,
                ProductSKU = row.Table.Columns.Contains("ProductSKU") ? row["ProductSKU"]?.ToString() : null
            };
        }
    }
}
