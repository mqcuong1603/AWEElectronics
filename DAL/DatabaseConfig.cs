namespace AWEElectronics.DAL
{
    public static class DatabaseConfig
    {
        // Docker SQL Server Connection String
        // Change this if using different server
        public static string ConnectionString =
            "Server=localhost,1433;" +
            "Database=AWEElectronics_DB;" +
            "User Id=sa;" +
            "Password=YourStrong@Password123;" +
            "TrustServerCertificate=True;";
    }
}
