using System;

class Program
{
    static void Main()
    {
        string password = "123456";
        string hash = BCrypt.Net.BCrypt.HashPassword(password);

        Console.WriteLine("Password: " + password);
        Console.WriteLine("Hash: " + hash);
        Console.WriteLine("\nVerification: " + BCrypt.Net.BCrypt.Verify(password, hash));
    }
}
