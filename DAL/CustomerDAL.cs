using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class CustomerDAL
    {
        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            string query = @"SELECT * FROM Customers ORDER BY RegisteredDate DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                customers.Add(MapCustomer(row));
            }
            return customers;
        }

        public Customer GetById(int customerId)
        {
            string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
            SqlParameter[] parameters = { new SqlParameter("@CustomerID", customerId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapCustomer(dt.Rows[0]);

            return null;
        }

        public Customer GetByEmail(string email)
        {
            string query = "SELECT * FROM Customers WHERE Email = @Email";
            SqlParameter[] parameters = { new SqlParameter("@Email", email) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapCustomer(dt.Rows[0]);

            return null;
        }

        public int Insert(Customer customer)
        {
            string query = @"INSERT INTO Customers (Email, PasswordHash, FullName, Phone, IsActive)
                            VALUES (@Email, @PasswordHash, @FullName, @Phone, @IsActive);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@PasswordHash", (object)customer.PasswordHash ?? DBNull.Value),
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@IsActive", customer.IsActive)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Update(Customer customer)
        {
            string query = @"UPDATE Customers 
                            SET Email = @Email, FullName = @FullName, Phone = @Phone, 
                                DefaultShippingAddressID = @DefaultShippingAddressID, IsActive = @IsActive
                            WHERE CustomerID = @CustomerID";

            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@Email", customer.Email),
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@DefaultShippingAddressID", (object)customer.DefaultShippingAddressID ?? DBNull.Value),
                new SqlParameter("@IsActive", customer.IsActive)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdatePassword(int customerId, string passwordHash)
        {
            string query = "UPDATE Customers SET PasswordHash = @PasswordHash WHERE CustomerID = @CustomerID";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@PasswordHash", passwordHash)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int customerId)
        {
            string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
            SqlParameter[] parameters = { new SqlParameter("@CustomerID", customerId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private Customer MapCustomer(DataRow row)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(row["CustomerID"]),
                Email = row["Email"].ToString(),
                PasswordHash = row["PasswordHash"] != DBNull.Value ? row["PasswordHash"].ToString() : null,
                FullName = row["FullName"].ToString(),
                Phone = row["Phone"].ToString(),
                DefaultShippingAddressID = row["DefaultShippingAddressID"] != DBNull.Value ? Convert.ToInt32(row["DefaultShippingAddressID"]) : (int?)null,
                IsActive = Convert.ToBoolean(row["IsActive"]),
                RegisteredDate = Convert.ToDateTime(row["RegisteredDate"])
            };
        }
    }
}
