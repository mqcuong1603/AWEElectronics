using System;
using AWEElectronics.DAL;
using AWEElectronics.DTO;

class DiagnosticLogin
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Diagnostic Login Test ===");
        Console.WriteLine();

        UserDAL userDAL = new UserDAL();

        Console.WriteLine("Testing database connection...");
        try
        {
            User user = userDAL.GetByUsername("admin");

            if (user == null)
            {
                Console.WriteLine("ERROR: User 'admin' not found in database!");
            }
            else
            {
                Console.WriteLine($"SUCCESS: Found user 'admin'");
                Console.WriteLine($"UserID: {user.UserID}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"FullName: {user.FullName}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine($"Role: {user.Role}");
                Console.WriteLine($"Status: {user.Status}");
                Console.WriteLine($"PasswordHash: {user.PasswordHash}");
                Console.WriteLine();

                Console.WriteLine("Testing BCrypt verification...");
                string password = "123456";
                Console.WriteLine($"Password to verify: {password}");

                try
                {
                    bool result = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                    Console.WriteLine($"BCrypt.Verify result: {result}");

                    if (result)
                    {
                        Console.WriteLine("SUCCESS: Password matches!");
                    }
                    else
                    {
                        Console.WriteLine("FAIL: Password does NOT match!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in BCrypt.Verify: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
        }

        Console.WriteLine();
        Console.WriteLine("=== Test Complete ===");
    }
}
