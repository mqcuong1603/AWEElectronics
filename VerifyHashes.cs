using System;
using System.Security.Cryptography;
using System.Text;

class VerifyHashes
{
    static void Main()
    {
        Console.WriteLine("Verifying SHA256 Hashes");
        Console.WriteLine("=======================\n");

        string[] passwords = { "admin123", "staff123", "agent123" };

        foreach (string password in passwords)
        {
            string hash = HashPassword(password);
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"SHA256:   {hash}");
            Console.WriteLine();
        }

        Console.WriteLine("\n=======================");
        Console.WriteLine("SQL Update Script:");
        Console.WriteLine("=======================\n");

        string adminHash = HashPassword("admin123");
        string staffHash = HashPassword("staff123");
        string agentHash = HashPassword("agent123");

        Console.WriteLine("UPDATE Users SET PasswordHash = '{0}' WHERE Username = 'admin';", adminHash);
        Console.WriteLine("UPDATE Users SET PasswordHash = '{0}' WHERE Username IN ('jsmith', 'mjones', 'slee');", staffHash);
        Console.WriteLine("UPDATE Users SET PasswordHash = '{0}' WHERE Username = 'bwilson';", agentHash);
    }

    static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
