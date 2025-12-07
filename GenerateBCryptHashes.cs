using System;
using BCrypt.Net;

class GenerateBCryptHashes
{
    static void Main()
    {
        Console.WriteLine("Generating BCrypt hashes for AWE Electronics users");
        Console.WriteLine("====================================================\n");

        // Generate hashes for different passwords
        string[] passwords = {
            "admin123",
            "staff123",
            "agent123"
        };

        foreach (string password in passwords)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine();
        }

        Console.WriteLine("\n====================================================");
        Console.WriteLine("SQL UPDATE Script:");
        Console.WriteLine("====================================================\n");

        // Generate specific hashes for each user
        string adminHash = BCrypt.Net.BCrypt.HashPassword("admin123", 12);
        string staffHash = BCrypt.Net.BCrypt.HashPassword("staff123", 12);
        string agentHash = BCrypt.Net.BCrypt.HashPassword("agent123", 12);

        Console.WriteLine("USE AWEElectronics_DB;");
        Console.WriteLine("GO\n");
        Console.WriteLine("-- Update admin user");
        Console.WriteLine($"UPDATE Users SET PasswordHash = '{adminHash}' WHERE Username = 'admin';");
        Console.WriteLine("\n-- Update staff users (jsmith, slee)");
        Console.WriteLine($"UPDATE Users SET PasswordHash = '{staffHash}' WHERE Username IN ('jsmith', 'slee');");
        Console.WriteLine("\n-- Update accountant user (mjones)");
        Console.WriteLine($"UPDATE Users SET PasswordHash = '{staffHash}' WHERE Username = 'mjones';");
        Console.WriteLine("\n-- Update agent user (bwilson)");
        Console.WriteLine($"UPDATE Users SET PasswordHash = '{agentHash}' WHERE Username = 'bwilson';");
        Console.WriteLine("\nGO\n");
        Console.WriteLine("-- Verify updates");
        Console.WriteLine("SELECT UserID, Username, FullName, Role, LEFT(PasswordHash, 30) + '...' as HashPreview");
        Console.WriteLine("FROM Users ORDER BY UserID;");
        Console.WriteLine("GO");

        Console.WriteLine("\n====================================================");
        Console.WriteLine("Login Credentials:");
        Console.WriteLine("====================================================");
        Console.WriteLine("admin    -> password: admin123");
        Console.WriteLine("jsmith   -> password: staff123");
        Console.WriteLine("mjones   -> password: staff123");
        Console.WriteLine("bwilson  -> password: agent123");
        Console.WriteLine("slee     -> password: staff123");
    }
}
