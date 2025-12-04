using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Configuration;

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

            // Test 4: Check Admin User and Password
            result.AppendLine("\n--- Test 4: Admin User Check ---");
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
                                result.AppendLine($"Password Hash: {hash.Substring(0, Math.Min(30, hash.Length))}...");
                                
                                // Calculate expected hash for "password123"
                                string expectedHash = CalculateSHA256("password123");
                                result.AppendLine($"\nExpected Hash for 'password123': {expectedHash.Substring(0, 30)}...");
                                result.AppendLine(hash.Equals(expectedHash, StringComparison.OrdinalIgnoreCase) 
                                    ? "? Password hash MATCHES expected value" 
                                    : "? Password hash does NOT match expected value");
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

            // Test 5: Session State
            result.AppendLine("\n--- Test 5: Session Configuration ---");
            result.AppendLine($"Session Available: {Session != null}");
            if (Session != null)
            {
                result.AppendLine("? Session is configured");
                result.AppendLine($"Session Mode: InProc");
                result.AppendLine($"Session Timeout: 60 minutes");
            }

            result.AppendLine("\n--- Test 6: Application Info ---");
            result.AppendLine($"Framework: .NET Framework 4.8");
            result.AppendLine($"MVC Version: 5.2.9");
            result.AppendLine($"Debug Mode: {System.Diagnostics.Debugger.IsAttached}");

            result.AppendLine("\n=== DIAGNOSTICS COMPLETE ===");
            result.AppendLine("\nIf all tests pass but login still fails:");
            result.AppendLine("1. Clear browser cache and cookies");
            result.AppendLine("2. Try username: admin (lowercase)");
            result.AppendLine("3. Try password: password123");
            result.AppendLine("4. Check browser console for JavaScript errors");
            result.AppendLine("5. Check Visual Studio Output window for exceptions");

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

        private string CalculateSHA256(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var builder = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
