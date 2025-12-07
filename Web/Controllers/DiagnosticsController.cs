using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class DiagnosticsController : Controller
    {
        // GET: Diagnostics
        public ActionResult Index()
        {
            var results = new StringBuilder();
            results.AppendLine("<html><head><title>AWE Electronics Login Diagnostics</title>");
            results.AppendLine("<style>");
            results.AppendLine("body { font-family: 'Segoe UI', Arial, sans-serif; margin: 20px; background: #f5f5f5; }");
            results.AppendLine("h1 { color: #2196F3; }");
            results.AppendLine("h2 { color: #333; margin-top: 30px; border-bottom: 2px solid #2196F3; padding-bottom: 5px; }");
            results.AppendLine(".test { background: white; padding: 15px; margin: 10px 0; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }");
            results.AppendLine(".success { border-left: 4px solid #4CAF50; }");
            results.AppendLine(".error { border-left: 4px solid #f44336; }");
            results.AppendLine(".warning { border-left: 4px solid #ff9800; }");
            results.AppendLine(".info { border-left: 4px solid #2196F3; }");
            results.AppendLine("pre { background: #f9f9f9; padding: 10px; border-radius: 3px; overflow-x: auto; }");
            results.AppendLine("table { border-collapse: collapse; width: 100%; margin: 10px 0; }");
            results.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            results.AppendLine("th { background-color: #2196F3; color: white; }");
            results.AppendLine("</style></head><body>");
            results.AppendLine("<h1>AWE Electronics - Login System Diagnostics</h1>");
            results.AppendLine($"<p>Test Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");

            // Test 1: Database Connection
            results.AppendLine("<h2>Test 1: Database Connection</h2>");
            try
            {
                // Try to get all users - if this works, database is connected
                UserBLL userBLL = new UserBLL();
                var users = userBLL.GetAll();

                results.AppendLine("<div class='test success'>");
                results.AppendLine($"<strong>✓ SUCCESS</strong> - Database connection established (found {users.Count} users)");
                results.AppendLine("</div>");
            }
            catch (Exception ex)
            {
                results.AppendLine("<div class='test error'>");
                results.AppendLine($"<strong>✗ ERROR</strong> - {ex.Message}<br>");
                results.AppendLine($"<pre>{ex.StackTrace}</pre>");
                results.AppendLine("</div>");
            }

            // Test 2: Query Users Table
            results.AppendLine("<h2>Test 2: Users Table Query</h2>");
            List<User> allUsers = new List<User>();
            try
            {
                UserBLL userBLL = new UserBLL();
                allUsers = userBLL.GetAll();

                results.AppendLine("<div class='test success'>");
                results.AppendLine($"<strong>✓ SUCCESS</strong> - Found {allUsers.Count} users<br><br>");
                results.AppendLine("<table>");
                results.AppendLine("<tr><th>ID</th><th>Username</th><th>Full Name</th><th>Email</th><th>Role</th><th>Status</th><th>Hash Preview</th><th>Hash Length</th></tr>");

                foreach (User user in allUsers)
                {
                    string hashPreview = user.PasswordHash.Length > 30 ?
                        user.PasswordHash.Substring(0, 30) + "..." :
                        user.PasswordHash;

                    results.AppendLine("<tr>");
                    results.AppendLine($"<td>{user.UserID}</td>");
                    results.AppendLine($"<td>{user.Username}</td>");
                    results.AppendLine($"<td>{user.FullName}</td>");
                    results.AppendLine($"<td>{user.Email}</td>");
                    results.AppendLine($"<td>{user.Role}</td>");
                    results.AppendLine($"<td>{user.Status}</td>");
                    results.AppendLine($"<td><code>{hashPreview}</code></td>");
                    results.AppendLine($"<td>{user.PasswordHash.Length}</td>");
                    results.AppendLine("</tr>");
                }
                results.AppendLine("</table>");
                results.AppendLine("</div>");
            }
            catch (Exception ex)
            {
                results.AppendLine("<div class='test error'>");
                results.AppendLine($"<strong>✗ ERROR</strong> - {ex.Message}<br>");
                results.AppendLine($"<pre>{ex.StackTrace}</pre>");
                results.AppendLine("</div>");
            }

            // Test 3: Get Full Hash for Admin User
            results.AppendLine("<h2>Test 3: Admin User Details & BCrypt Test</h2>");
            string adminHash = "";
            User adminUser = null;
            try
            {
                adminUser = allUsers.Find(u => u.Username == "admin");

                if (adminUser != null)
                {
                    adminHash = adminUser.PasswordHash;
                    results.AppendLine("<div class='test success'>");
                    results.AppendLine("<strong>✓ SUCCESS</strong> - Retrieved admin user<br>");
                    results.AppendLine($"Username: {adminUser.Username}<br>");
                    results.AppendLine($"Full Hash: <code>{adminHash}</code><br>");
                    results.AppendLine($"Hash Length: {adminHash.Length} characters<br>");
                    results.AppendLine($"Hash starts with: {adminHash.Substring(0, Math.Min(10, adminHash.Length))}<br>");
                    results.AppendLine($"Hash has leading spaces: {adminHash.StartsWith(" ")}<br>");
                    results.AppendLine($"Hash has trailing spaces: {adminHash.EndsWith(" ")}<br>");
                    results.AppendLine("</div>");

                    // Direct BCrypt test right here
                    results.AppendLine("<div class='test info'>");
                    results.AppendLine("<strong>Direct BCrypt Verification Test:</strong><br><br>");

                    string testPassword = "123456";
                    results.AppendLine($"Testing password: '<strong>{testPassword}</strong>'<br>");
                    results.AppendLine($"Against hash: <code>{adminHash}</code><br><br>");

                    try
                    {
                        // Try with BCrypt directly
                        bool bcryptResult = BCrypt.Net.BCrypt.Verify(testPassword, adminHash.Trim());
                        results.AppendLine($"BCrypt.Verify() result: <strong style='color: {(bcryptResult ? "green" : "red")};'>{bcryptResult}</strong><br>");

                        if (!bcryptResult)
                        {
                            results.AppendLine("<br><strong style='color: orange;'>Testing with trimmed hash...</strong><br>");
                            bool trimmedResult = BCrypt.Net.BCrypt.Verify(testPassword, adminHash.Trim());
                            results.AppendLine($"BCrypt.Verify(trimmed) result: <strong>{trimmedResult}</strong><br>");
                        }
                    }
                    catch (Exception bcryptEx)
                    {
                        results.AppendLine($"<strong style='color: red;'>BCrypt Error:</strong> {bcryptEx.Message}<br>");
                        results.AppendLine($"<pre>{bcryptEx.StackTrace}</pre>");
                    }

                    results.AppendLine("</div>");
                }
                else
                {
                    results.AppendLine("<div class='test error'>");
                    results.AppendLine("<strong>✗ FAILED</strong> - Admin user not found");
                    results.AppendLine("</div>");
                }
            }
            catch (Exception ex)
            {
                results.AppendLine("<div class='test error'>");
                results.AppendLine($"<strong>✗ ERROR</strong> - {ex.Message}");
                results.AppendLine("</div>");
            }

            // Test 4: UserBLL Login Test
            results.AppendLine("<h2>Test 4: UserBLL.Login() Method Test</h2>");
            try
            {
                UserBLL userBLL = new UserBLL();
                string[] testCredentials = {
                    "admin|123456",
                    "admin|admin",
                    "admin|admin123",
                    "jsmith|123456"
                };

                results.AppendLine("<div class='test info'>");
                results.AppendLine("<strong>Testing login attempts:</strong><br><br>");
                results.AppendLine("<table>");
                results.AppendLine("<tr><th>Username</th><th>Password</th><th>Success</th><th>Message</th><th>User Details</th></tr>");

                foreach (string cred in testCredentials)
                {
                    string[] parts = cred.Split('|');
                    string username = parts[0];
                    string password = parts[1];

                    try
                    {
                        var loginResult = userBLL.Login(username, password);
                        results.AppendLine("<tr>");
                        results.AppendLine($"<td>{username}</td>");
                        results.AppendLine($"<td>{password}</td>");

                        if (loginResult.Success)
                        {
                            results.AppendLine("<td style='color: green;'><strong>✓ SUCCESS</strong></td>");
                            results.AppendLine($"<td>{loginResult.Message}</td>");
                            results.AppendLine($"<td>{loginResult.User.FullName} ({loginResult.User.Role})</td>");
                        }
                        else
                        {
                            results.AppendLine("<td style='color: red;'><strong>✗ FAILED</strong></td>");
                            results.AppendLine($"<td>{loginResult.Message}</td>");
                            results.AppendLine("<td>-</td>");
                        }
                        results.AppendLine("</tr>");
                    }
                    catch (Exception ex)
                    {
                        results.AppendLine("<tr>");
                        results.AppendLine($"<td>{username}</td>");
                        results.AppendLine($"<td>{password}</td>");
                        results.AppendLine("<td style='color: orange;'><strong>ERROR</strong></td>");
                        results.AppendLine($"<td colspan='2'>{ex.Message}</td>");
                        results.AppendLine("</tr>");
                    }
                }
                results.AppendLine("</table>");
                results.AppendLine("</div>");
            }
            catch (Exception ex)
            {
                results.AppendLine("<div class='test error'>");
                results.AppendLine($"<strong>✗ ERROR</strong> - {ex.Message}<br>");
                results.AppendLine($"<pre>{ex.StackTrace}</pre>");
                results.AppendLine("</div>");
            }

            // Test 6: AccountController Login Test
            results.AppendLine("<h2>Test 6: Quick Login Form Test</h2>");
            results.AppendLine("<div class='test info'>");
            results.AppendLine("<strong>Test the actual login form:</strong><br><br>");
            results.AppendLine("<form action='/Account/Login' method='post'>");
            results.AppendLine("<input type='hidden' name='__RequestVerificationToken' value='test' />");
            results.AppendLine("Username: <input type='text' name='Username' value='admin' style='padding: 5px; margin: 5px;' /><br>");
            results.AppendLine("Password: <input type='password' name='Password' value='123456' style='padding: 5px; margin: 5px;' /><br>");
            results.AppendLine("<input type='checkbox' name='RememberMe' value='false' /> Remember Me<br><br>");
            results.AppendLine("<button type='submit' style='padding: 10px 20px; background: #2196F3; color: white; border: none; cursor: pointer;'>Test Login</button>");
            results.AppendLine("</form>");
            results.AppendLine("<br><p><a href='/Account/Login'>Go to actual login page</a></p>");
            results.AppendLine("</div>");

            results.AppendLine("</body></html>");

            return Content(results.ToString(), "text/html");
        }

    }
}
