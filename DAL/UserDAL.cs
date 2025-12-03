using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AWEElectronics.DTO;

namespace AWEElectronics.DAL
{
    public class UserDAL
    {
        public User GetByUsername(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = { new SqlParameter("@Username", username) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapUser(dt.Rows[0]);

            return null;
        }

        public User GetById(int userId)
        {
            string query = "SELECT * FROM Users WHERE UserID = @UserID";
            SqlParameter[] parameters = { new SqlParameter("@UserID", userId) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
                return MapUser(dt.Rows[0]);

            return null;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM Users ORDER BY FullName";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                users.Add(MapUser(row));
            }
            return users;
        }

        public List<User> GetByRole(string role)
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM Users WHERE Role = @Role ORDER BY FullName";
            SqlParameter[] parameters = { new SqlParameter("@Role", role) };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                users.Add(MapUser(row));
            }
            return users;
        }

        public int Insert(User user)
        {
            string query = @"INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, Status)
                            VALUES (@Username, @PasswordHash, @FullName, @Email, @Role, @Status);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@Status", user.Status ?? "Active")
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        public bool Update(User user)
        {
            string query = @"UPDATE Users SET
                            FullName = @FullName,
                            Email = @Email,
                            Role = @Role,
                            Status = @Status
                            WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", user.UserID),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@Status", user.Status)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdatePassword(int userId, string newPasswordHash)
        {
            string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";
            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PasswordHash", newPasswordHash)
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int userId)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            SqlParameter[] parameters = { new SqlParameter("@UserID", userId) };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private User MapUser(DataRow row)
        {
            return new User
            {
                UserID = Convert.ToInt32(row["UserID"]),
                Username = row["Username"].ToString(),
                PasswordHash = row["PasswordHash"].ToString(),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Role = row["Role"].ToString(),
                Status = row["Status"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }
    }
}
