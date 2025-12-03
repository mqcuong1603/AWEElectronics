using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class ProductDAL
    {
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            ORDER BY p.Name";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                products.Add(MapProduct(row));
            }
            return products;
        }

        public List<Product> GetPublished()
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.IsPublished = 1
                            ORDER BY p.Name";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                products.Add(MapProduct(row));
            }
            return products;
        }

        public Product GetById(int productId)
        {
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.ProductID = @ProductID";

            SqlParameter[] parameters = { new SqlParameter("@ProductID", productId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapProduct(dt.Rows[0]);

            return null;
        }

        public Product GetBySKU(string sku)
        {
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.SKU = @SKU";

            SqlParameter[] parameters = { new SqlParameter("@SKU", sku) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapProduct(dt.Rows[0]);

            return null;
        }

        public List<Product> GetByCategory(int categoryId)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.CategoryID = @CategoryID
                            ORDER BY p.Name";

            SqlParameter[] parameters = { new SqlParameter("@CategoryID", categoryId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                products.Add(MapProduct(row));
            }
            return products;
        }

        public List<Product> Search(string keyword)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.Name LIKE @Keyword OR p.SKU LIKE @Keyword
                            ORDER BY p.Name";

            SqlParameter[] parameters = { new SqlParameter("@Keyword", $"%{keyword}%") };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                products.Add(MapProduct(row));
            }
            return products;
        }

        public List<Product> GetLowStock(int threshold = 10)
        {
            List<Product> products = new List<Product>();
            string query = @"SELECT p.*, c.Name as CategoryName, s.CompanyName as SupplierName
                            FROM Products p
                            LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                            LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID
                            WHERE p.StockLevel <= @Threshold
                            ORDER BY p.StockLevel ASC";

            SqlParameter[] parameters = { new SqlParameter("@Threshold", threshold) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                products.Add(MapProduct(row));
            }
            return products;
        }

        public int Insert(Product product)
        {
            string query = @"INSERT INTO Products (CategoryID, SupplierID, SKU, Name, Specifications, Price, StockLevel, IsPublished)
                            VALUES (@CategoryID, @SupplierID, @SKU, @Name, @Specifications, @Price, @StockLevel, @IsPublished);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", product.CategoryID),
                new SqlParameter("@SupplierID", (object)product.SupplierID ?? DBNull.Value),
                new SqlParameter("@SKU", product.SKU),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Specifications", (object)product.Specifications ?? DBNull.Value),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@StockLevel", product.StockLevel),
                new SqlParameter("@IsPublished", product.IsPublished)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Update(Product product)
        {
            string query = @"UPDATE Products SET
                            CategoryID = @CategoryID,
                            SupplierID = @SupplierID,
                            SKU = @SKU,
                            Name = @Name,
                            Specifications = @Specifications,
                            Price = @Price,
                            StockLevel = @StockLevel,
                            IsPublished = @IsPublished
                            WHERE ProductID = @ProductID";

            SqlParameter[] parameters = {
                new SqlParameter("@ProductID", product.ProductID),
                new SqlParameter("@CategoryID", product.CategoryID),
                new SqlParameter("@SupplierID", (object)product.SupplierID ?? DBNull.Value),
                new SqlParameter("@SKU", product.SKU),
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Specifications", (object)product.Specifications ?? DBNull.Value),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@StockLevel", product.StockLevel),
                new SqlParameter("@IsPublished", product.IsPublished)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateStock(int productId, int quantityChange)
        {
            string query = "UPDATE Products SET StockLevel = StockLevel + @Quantity WHERE ProductID = @ProductID";
            SqlParameter[] parameters = {
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@Quantity", quantityChange)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int productId)
        {
            string query = "DELETE FROM Products WHERE ProductID = @ProductID";
            SqlParameter[] parameters = { new SqlParameter("@ProductID", productId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private Product MapProduct(DataRow row)
        {
            return new Product
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                SupplierID = row["SupplierID"] != DBNull.Value ? Convert.ToInt32(row["SupplierID"]) : (int?)null,
                SKU = row["SKU"].ToString(),
                Name = row["Name"].ToString(),
                Specifications = row["Specifications"]?.ToString(),
                Price = Convert.ToDecimal(row["Price"]),
                StockLevel = Convert.ToInt32(row["StockLevel"]),
                IsPublished = Convert.ToBoolean(row["IsPublished"]),
                CategoryName = row.Table.Columns.Contains("CategoryName") ? row["CategoryName"]?.ToString() : null,
                SupplierName = row.Table.Columns.Contains("SupplierName") ? row["SupplierName"]?.ToString() : null
            };
        }
    }
}
