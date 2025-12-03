using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class SupplierDAL
    {
        public List<Supplier> GetAll()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string query = "SELECT * FROM Suppliers ORDER BY CompanyName";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                suppliers.Add(MapSupplier(row));
            }
            return suppliers;
        }

        public Supplier GetById(int supplierId)
        {
            string query = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
            SqlParameter[] parameters = { new SqlParameter("@SupplierID", supplierId) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapSupplier(dt.Rows[0]);

            return null;
        }

        public int Insert(Supplier supplier)
        {
            string query = @"INSERT INTO Suppliers (CompanyName, ContactName, Phone, Email, Address)
                            VALUES (@CompanyName, @ContactName, @Phone, @Email, @Address);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CompanyName", supplier.CompanyName),
                new SqlParameter("@ContactName", (object)supplier.ContactName ?? DBNull.Value),
                new SqlParameter("@Phone", (object)supplier.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object)supplier.Email ?? DBNull.Value),
                new SqlParameter("@Address", (object)supplier.Address ?? DBNull.Value)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Update(Supplier supplier)
        {
            string query = @"UPDATE Suppliers SET
                            CompanyName = @CompanyName,
                            ContactName = @ContactName,
                            Phone = @Phone,
                            Email = @Email,
                            Address = @Address
                            WHERE SupplierID = @SupplierID";

            SqlParameter[] parameters = {
                new SqlParameter("@SupplierID", supplier.SupplierID),
                new SqlParameter("@CompanyName", supplier.CompanyName),
                new SqlParameter("@ContactName", (object)supplier.ContactName ?? DBNull.Value),
                new SqlParameter("@Phone", (object)supplier.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object)supplier.Email ?? DBNull.Value),
                new SqlParameter("@Address", (object)supplier.Address ?? DBNull.Value)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int supplierId)
        {
            string query = "DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
            SqlParameter[] parameters = { new SqlParameter("@SupplierID", supplierId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private Supplier MapSupplier(DataRow row)
        {
            return new Supplier
            {
                SupplierID = Convert.ToInt32(row["SupplierID"]),
                CompanyName = row["CompanyName"].ToString(),
                ContactName = row["ContactName"]?.ToString(),
                Phone = row["Phone"]?.ToString(),
                Email = row["Email"]?.ToString(),
                Address = row["Address"]?.ToString()
            };
        }
    }
}
