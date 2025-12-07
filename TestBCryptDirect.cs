using System;
using BCrypt.Net;

class TestBCryptDirect
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Testing BCrypt.Net Directly ===");
        Console.WriteLine();

        // The hash from the database
        string storedHash = "$2a$11$/gr01j4lbyRe74e8X8.e1uKb.ak2e.WXXw9qSTFGrWByp11v7qRGW";
        string password = "123456";

        Console.WriteLine($"Hash from DB: {storedHash}");
        Console.WriteLine($"Password: {password}");
        Console.WriteLine();

        try
        {
            bool result = BCrypt.Net.BCrypt.Verify(password, storedHash);
            Console.WriteLine($"BCrypt.Verify result: {result}");

            if (result)
            {
                Console.WriteLine("SUCCESS: Password matches hash!");
            }
            else
            {
                Console.WriteLine("FAIL: Password does NOT match hash!");
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
