using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class InventoryTransactionDAL
    {
        public List<InventoryTransaction> GetAll()
        {
            List<InventoryTransaction> transactions = new List<InventoryTransaction>();
            string query = @"SELECT it.*, p.Name as ProductName, p.SKU as ProductSKU, u.FullName as PerformedByName
                            FROM InventoryTransactions it
                            LEFT JOIN Products p ON it.ProductID = p.ProductID
                            LEFT JOIN Users u ON it.PerformedBy = u.UserID
                            ORDER BY it.TransDate DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                transactions.Add(MapTransaction(row));
            }
            return transactions;
        }

        public List<InventoryTransaction> GetByProductId(int productId)
        {
            List<InventoryTransaction> transactions = new List<InventoryTransaction>();
            string query = @"SELECT it.*, p.Name as ProductName, p.SKU as ProductSKU, u.FullName as PerformedByName
                            FROM InventoryTransactions it
                            LEFT JOIN Products p ON it.ProductID = p.ProductID
                            LEFT JOIN Users u ON it.PerformedBy = u.UserID
                            WHERE it.ProductID = @ProductID
                            ORDER BY it.TransDate DESC";

            SqlParameter[] parameters = { new SqlParameter("@ProductID", productId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                transactions.Add(MapTransaction(row));
            }
            return transactions;
        }

        public List<InventoryTransaction> GetByType(string type)
        {
            List<InventoryTransaction> transactions = new List<InventoryTransaction>();
            string query = @"SELECT it.*, p.Name as ProductName, p.SKU as ProductSKU, u.FullName as PerformedByName
                            FROM InventoryTransactions it
                            LEFT JOIN Products p ON it.ProductID = p.ProductID
                            LEFT JOIN Users u ON it.PerformedBy = u.UserID
                            WHERE it.Type = @Type
                            ORDER BY it.TransDate DESC";

            SqlParameter[] parameters = { new SqlParameter("@Type", type) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                transactions.Add(MapTransaction(row));
            }
            return transactions;
        }

        public int Insert(InventoryTransaction transaction)
        {
            string query = @"INSERT INTO InventoryTransactions (ProductID, Type, Quantity, ReferenceNumber, PerformedBy, Notes)
                            VALUES (@ProductID, @Type, @Quantity, @ReferenceNumber, @PerformedBy, @Notes);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@ProductID", transaction.ProductID),
                new SqlParameter("@Type", transaction.Type),
                new SqlParameter("@Quantity", transaction.Quantity),
                new SqlParameter("@ReferenceNumber", (object)transaction.ReferenceNumber ?? DBNull.Value),
                new SqlParameter("@PerformedBy", transaction.PerformedBy),
                new SqlParameter("@Notes", (object)transaction.Notes ?? DBNull.Value)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public string GenerateReferenceNumber(string type)
        {
            string prefix = type == "IN" ? "GRN" : (type == "OUT" ? "GDN" : "ADJ");
            return $"{prefix}-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

        private InventoryTransaction MapTransaction(DataRow row)
        {
            return new InventoryTransaction
            {
                TransID = Convert.ToInt32(row["TransID"]),
                ProductID = Convert.ToInt32(row["ProductID"]),
                Type = row["Type"].ToString(),
                Quantity = Convert.ToInt32(row["Quantity"]),
                ReferenceNumber = row["ReferenceNumber"]?.ToString(),
                PerformedBy = Convert.ToInt32(row["PerformedBy"]),
                TransDate = Convert.ToDateTime(row["TransDate"]),
                Notes = row["Notes"]?.ToString(),
                ProductName = row.Table.Columns.Contains("ProductName") ? row["ProductName"]?.ToString() : null,
                ProductSKU = row.Table.Columns.Contains("ProductSKU") ? row["ProductSKU"]?.ToString() : null,
                PerformedByName = row.Table.Columns.Contains("PerformedByName") ? row["PerformedByName"]?.ToString() : null
            };
        }
    }
}
