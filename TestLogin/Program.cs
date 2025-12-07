using System;
using AWEElectronics.BLL;

namespace TestLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing AWE Electronics Login");
            Console.WriteLine("=============================\n");

            UserBLL userBLL = new UserBLL();

            // Test with password found from Python test
            string username = "admin";
            string password = "123456";

            Console.WriteLine($"Attempting login with username: {username}");
            Console.WriteLine($"Password: {password}\n");

            try
            {
                var result = userBLL.Login(username, password);

                Console.WriteLine($"Success: {result.Success}");
                Console.WriteLine($"Message: {result.Message}");

                if (result.Success && result.User != null)
                {
                    Console.WriteLine($"\nUser Details:");
                    Console.WriteLine($"  UserID: {result.User.UserID}");
                    Console.WriteLine($"  Username: {result.User.Username}");
                    Console.WriteLine($"  Full Name: {result.User.FullName}");
                    Console.WriteLine($"  Email: {result.User.Email}");
                    Console.WriteLine($"  Role: {result.User.Role}");
                    Console.WriteLine($"  Status: {result.User.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
