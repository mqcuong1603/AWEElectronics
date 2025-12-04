using System.Configuration;

namespace AWEElectronics.DAL
{
    public static class DatabaseConfig
    {
        /// <summary>
        /// Gets the database connection string from Web.config or falls back to default
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                // Try to read from Web.config first (for web deployment)
                var configConnectionString = ConfigurationManager.ConnectionStrings["AWEElectronics"]?.ConnectionString;
                
                if (!string.IsNullOrEmpty(configConnectionString))
                {
                    return configConnectionString;
                }
                
                // Fallback to default (for local development with Docker)
                return "Server=localhost,1433;" +
                       "Database=AWEElectronics_DB;" +
                       "User Id=sa;" +
                       "Password=YourStrong@Password123;" +
                       "TrustServerCertificate=True;";
            }
        }
    }
}
