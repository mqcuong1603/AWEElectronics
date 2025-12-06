using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Configuration;
using AWEElectronics.BLL;
using BCrypt.Net;

namespace Web.Controllers
{
    public class DiagnosticsController : Controller
    {
        // GET: /Diagnostics/TestConnection
        public ActionResult TestConnection()
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("=== AWE Electronics - System Diagnostics ===\n");
            result.AppendLine($"Test Time: {DateTime.Now}\n");

            // Get connection string
            string connectionString = ConfigurationManager.ConnectionStrings["AWEElectronics"]?.ConnectionString;
            
            // Test 1: Connection String Check
            result.AppendLine("--- Test 1: Connection String Configuration ---");
            if (string.IsNullOrEmpty(connectionString))
            {
                result.AppendLine("? Connection string 'AWEElectronics' NOT FOUND in Web.config");
                return Content(result.ToString(), "text/plain");
            }
            else
            {
                result.AppendLine("? Connection string found");
                // Mask password for security
                var maskedConn = connectionString.Replace(GetPasswordFromConnectionString(connectionString), "****");
                result.AppendLine($"Connection: {maskedConn}");
            }

            // Test 2: Database Connection
            result.AppendLine("\n--- Test 2: Database Connection ---");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    result.AppendLine("? Database connection: SUCCESS");
                    result.AppendLine($"Database: {conn.Database}");
                    result.AppendLine($"Server: {conn.DataSource}");
                    result.AppendLine($"State: {conn.State}");
                    result.AppendLine($"Server Version: {conn.ServerVersion}");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"? Database connection: FAILED");
                result.AppendLine($"Error: {ex.Message}");
                
                if (ex.InnerException != null)
                {
                    result.AppendLine($"Inner Error: {ex.InnerException.Message}");
                }
                
                return Content(result.ToString(), "text/plain");
            }

            // Test 3: Users Table Check
            result.AppendLine("\n--- Test 3: Users Table Check ---");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if Users table exists
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'", conn))
                    {
                        int tableExists = (int)cmd.ExecuteScalar();
                        if (tableExists == 0)
                        {
                            result.AppendLine("? Users table does NOT exist");
                            return Content(result.ToString(), "text/plain");
                        }
                        result.AppendLine("? Users table exists");
                    }

                    // Count users
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        result.AppendLine($"? Total users in database: {count}");
                    }

                    // Check for specific users
                    using (SqlCommand cmd = new SqlCommand("SELECT Username, Role, Status FROM Users", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            result.AppendLine("\nUsers found:");
                            while (reader.Read())
                            {
                                result.AppendLine($"  - Username: {reader["Username"]}, Role: {reader["Role"]}, Status: {reader["Status"]}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"? Users table check: FAILED");
                result.AppendLine($"Error: {ex.Message}");
            }

            // Test 4: Check Admin User and BCrypt Password
            result.AppendLine("\n--- Test 4: Admin User & BCrypt Test ---");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Username, PasswordHash, Status FROM Users WHERE LOWER(Username) = 'admin'", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result.AppendLine("? Admin user exists");
                                result.AppendLine($"Username: {reader["Username"]}");
                                result.AppendLine($"Status: {reader["Status"]}");
                                string hash = reader["PasswordHash"].ToString();
                                result.AppendLine($"Password Hash Format: {hash.Substring(0, Math.Min(10, hash.Length))}... (BCrypt)");
                                result.AppendLine($"Hash Length: {hash.Length} characters");

                                // Test BCrypt verification with password "123456"
                                result.AppendLine("\nTesting BCrypt verification with password '123456':");
                                try
                                {
                                    bool isValid = BCrypt.Net.BCrypt.Verify("123456", hash);
                                    result.AppendLine(isValid
                                        ? "? BCrypt verification: SUCCESS - Password '123456' is correct!"
                                        : "? BCrypt verification: FAILED - Password does not match");
                                }
                                catch (Exception bcryptEx)
                                {
                                    result.AppendLine($"? BCrypt verification: ERROR - {bcryptEx.Message}");
                                }
                            }
                            else
                            {
                                result.AppendLine("? Admin user NOT found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"? Admin check: FAILED");
                result.AppendLine($"Error: {ex.Message}");
            }

            // Test 5: Full Login Test
            result.AppendLine("\n--- Test 5: Full Login Simulation ---");
            try
            {
                var userBLL = new UserBLL();
                var loginResult = userBLL.Login("admin", "123456");

                result.AppendLine($"Login Success: {loginResult.Success}");
                result.AppendLine($"Message: {loginResult.Message}");
                if (loginResult.Success && loginResult.User != null)
                {
                    result.AppendLine($"User ID: {loginResult.User.UserID}");
                    result.AppendLine($"Full Name: {loginResult.User.FullName}");
                    result.AppendLine($"Role: {loginResult.User.Role}");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"? Login test: FAILED");
                result.AppendLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    result.AppendLine($"Inner Error: {ex.InnerException.Message}");
                }
                result.AppendLine($"Stack Trace: {ex.StackTrace}");
            }

            // Test 6: Session State
            result.AppendLine("\n--- Test 6: Session Configuration ---");
            result.AppendLine($"Session Available: {Session != null}");
            if (Session != null)
            {
                result.AppendLine("? Session is configured");
                result.AppendLine($"Session Mode: InProc");
                result.AppendLine($"Session Timeout: 60 minutes");
            }

            result.AppendLine("\n--- Test 7: Application Info ---");
            result.AppendLine($"Framework: .NET Framework 4.8");
            result.AppendLine($"MVC Version: 5.2.9");
            result.AppendLine($"Debug Mode: {System.Diagnostics.Debugger.IsAttached}");

            result.AppendLine("\n=== DIAGNOSTICS COMPLETE ===");
            result.AppendLine("\nLogin Credentials:");
            result.AppendLine("  Username: admin");
            result.AppendLine("  Password: 123456");
            result.AppendLine("\nIf all tests pass but login still fails:");
            result.AppendLine("1. Clear browser cache and cookies");
            result.AppendLine("2. Make sure you're using username: admin (lowercase)");
            result.AppendLine("3. Make sure you're using password: 123456");
            result.AppendLine("4. Check browser console for JavaScript errors");
            result.AppendLine("5. Check the error message shown on the login page");

            return Content(result.ToString(), "text/plain");
        }

        // GET: /Diagnostics/FixPasswords
        public ActionResult FixPasswords()
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("=== PASSWORD HASH FIX ===\n");

            try
            {
                // Generate correct BCrypt hash for password "123456"
                string correctHash = BCrypt.Net.BCrypt.HashPassword("123456");

                result.AppendLine($"Generating new BCrypt hash for password: 123456");
                result.AppendLine($"New Hash: {correctHash}");

                // Verify the hash works
                bool verified = BCrypt.Net.BCrypt.Verify("123456", correctHash);
                result.AppendLine($"Hash Verification: {verified}\n");

                // Update all users in database
                string connectionString = ConfigurationManager.ConnectionStrings["AWEElectronics"]?.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Update all users with the correct hash
                    using (SqlCommand cmd = new SqlCommand("UPDATE Users SET PasswordHash = @PasswordHash", conn))
                    {
                        cmd.Parameters.AddWithValue("@PasswordHash", correctHash);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        result.AppendLine($"Database Update: {rowsAffected} user(s) updated");
                    }

                    // Verify the update
                    using (SqlCommand cmd = new SqlCommand("SELECT Username, PasswordHash FROM Users", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            result.AppendLine("\n--- Updated Users ---");
                            while (reader.Read())
                            {
                                string username = reader["Username"].ToString();
                                string hash = reader["PasswordHash"].ToString();
                                bool canLogin = BCrypt.Net.BCrypt.Verify("123456", hash);
                                result.AppendLine($"{username}: Can login with '123456' = {canLogin}");
                            }
                        }
                    }
                }

                result.AppendLine("\n✓ All users can now login with password: 123456");
                result.AppendLine("\nTry logging in with:");
                result.AppendLine("  Username: admin");
                result.AppendLine("  Password: 123456");
            }
            catch (Exception ex)
            {
                result.AppendLine($"\nERROR: {ex.Message}");
                result.AppendLine($"Stack: {ex.StackTrace}");
            }

            return Content(result.ToString(), "text/plain");
        }

        // GET: /Diagnostics/GenerateHash
        public ActionResult GenerateHash(string password = "123456")
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("=== BCrypt Hash Generator ===\n");

            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(password);
                result.AppendLine($"Password: {password}");
                result.AppendLine($"Generated Hash: {hash}");
                result.AppendLine($"Hash Length: {hash.Length}");

                // Verify it works
                bool verified = BCrypt.Net.BCrypt.Verify(password, hash);
                result.AppendLine($"\nVerification Test: {verified}");

                result.AppendLine($"\n--- SQL Update Command ---");
                result.AppendLine($"UPDATE Users SET PasswordHash = '{hash}';");
            }
            catch (Exception ex)
            {
                result.AppendLine($"ERROR: {ex.Message}");
            }

            return Content(result.ToString(), "text/plain");
        }

        // GET: /Diagnostics/QuickTest
        public ActionResult QuickTest()
        {
            var result = new System.Text.StringBuilder();
            result.AppendLine("=== QUICK LOGIN TEST ===\n");

            // The exact hash from database
            string dbHash = "$2a$12$LmcMslYkTqLELm0N.F2Wl.vx5N0H9Sq8KJp6pHQnFOg.zRlLJsLKu";

            result.AppendLine("--- Test 1: BCrypt Library Test ---");
            try
            {
                // Test with password "123456"
                bool test1 = BCrypt.Net.BCrypt.Verify("123456", dbHash);
                result.AppendLine($"BCrypt.Verify('123456', hash): {test1}");

                // Test with password "admin"
                bool test2 = BCrypt.Net.BCrypt.Verify("admin", dbHash);
                result.AppendLine($"BCrypt.Verify('admin', hash): {test2}");

                result.AppendLine("\nHash from DB: " + dbHash);
                result.AppendLine("Hash length: " + dbHash.Length);
            }
            catch (Exception ex)
            {
                result.AppendLine($"ERROR: {ex.Message}");
                result.AppendLine($"Stack: {ex.StackTrace}");
            }

            result.AppendLine("\n--- Test 2: Database Query Test ---");
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["AWEElectronics"]?.ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Username, PasswordHash, Status FROM Users WHERE LOWER(Username) = LOWER(@Username)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", "admin");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader["Username"].ToString();
                                string hash = reader["PasswordHash"].ToString();
                                string status = reader["Status"].ToString();

                                result.AppendLine($"Username from DB: '{username}'");
                                result.AppendLine($"Status from DB: '{status}'");
                                result.AppendLine($"Hash from DB: {hash}");
                                result.AppendLine($"Hash length: {hash.Length}");
                                result.AppendLine($"Hashes match: {hash == dbHash}");

                                result.AppendLine("\nBCrypt verification against DB hash:");
                                bool verified = BCrypt.Net.BCrypt.Verify("123456", hash);
                                result.AppendLine($"Result: {verified}");
                            }
                            else
                            {
                                result.AppendLine("User 'admin' NOT FOUND in database!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"ERROR: {ex.Message}");
            }

            result.AppendLine("\n--- Test 3: Full UserBLL.Login Test ---");
            try
            {
                var userBLL = new UserBLL();
                var loginResult = userBLL.Login("admin", "123456");

                result.AppendLine($"Login Success: {loginResult.Success}");
                result.AppendLine($"Message: {loginResult.Message}");

                if (loginResult.User != null)
                {
                    result.AppendLine($"User ID: {loginResult.User.UserID}");
                    result.AppendLine($"Username: {loginResult.User.Username}");
                    result.AppendLine($"FullName: {loginResult.User.FullName}");
                    result.AppendLine($"Role: {loginResult.User.Role}");
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"ERROR: {ex.Message}");
                result.AppendLine($"Stack: {ex.StackTrace}");
            }

            result.AppendLine("\n=== TEST COMPLETE ===");

            return Content(result.ToString(), "text/plain");
        }

        private string GetPasswordFromConnectionString(string connectionString)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                return builder.Password;
            }
            catch
            {
                return "";
            }
        }

    }
}
