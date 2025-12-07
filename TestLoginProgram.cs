using System;
using AWEElectronics.BLL;

namespace TestLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Testing Login with BCrypt ===");
            Console.WriteLine();

            UserBLL userBLL = new UserBLL();

            // Test 1: Valid login with admin/123456
            Console.WriteLine("Test 1: Logging in with admin/123456");
            var result1 = userBLL.Login("admin", "123456");
            Console.WriteLine($"Success: {result1.Success}");
            Console.WriteLine($"Message: {result1.Message}");
            if (result1.User != null)
            {
                Console.WriteLine($"User: {result1.User.FullName} ({result1.User.Role})");
            }
            Console.WriteLine();

            // Test 2: Invalid password
            Console.WriteLine("Test 2: Logging in with admin/wrongpassword");
            var result2 = userBLL.Login("admin", "wrongpassword");
            Console.WriteLine($"Success: {result2.Success}");
            Console.WriteLine($"Message: {result2.Message}");
            Console.WriteLine();

            // Test 3: Another valid user
            Console.WriteLine("Test 3: Logging in with jsmith/123456");
            var result3 = userBLL.Login("jsmith", "123456");
            Console.WriteLine($"Success: {result3.Success}");
            Console.WriteLine($"Message: {result3.Message}");
            if (result3.User != null)
            {
                Console.WriteLine($"User: {result3.User.FullName} ({result3.User.Role})");
            }
            Console.WriteLine();

            Console.WriteLine("=== Tests Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
