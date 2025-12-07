using System;
using BCrypt.Net;

class TestBCrypt
{
    static void Main()
    {
        // Hash from database
        string storedHash = "$2a$11$xmUVpf71/YynudMDR1VdauOOuBI.bKjUhfK5LDmxjKRs4Ps2kEgHS";

        // Common passwords to test
        string[] testPasswords = {
            "admin",
            "admin123",
            "Admin123",
            "password",
            "Password123",
            "YourStrong@Password123",
            "Admin",
            "Administrator",
            "123456",
            "awe123",
            "AWE123"
        };

        Console.WriteLine("Testing BCrypt hash: " + storedHash);
        Console.WriteLine();

        foreach (string password in testPasswords)
        {
            try
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(password, storedHash);
                Console.WriteLine($"Password '{password}': {(isValid ? "MATCH!" : "No match")}");

                if (isValid)
                {
                    Console.WriteLine("\n*** FOUND THE PASSWORD! ***");
                    Console.WriteLine($"The password is: {password}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Password '{password}': Error - {ex.Message}");
            }
        }

        Console.WriteLine("\nNo matching password found from the test list.");
        Console.WriteLine("You may need to reset the passwords in the database.");
    }
}
