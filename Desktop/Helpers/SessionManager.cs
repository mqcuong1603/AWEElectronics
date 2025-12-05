using AWEElectronics.DTO;

namespace Desktop.Helpers
{
    /// <summary>
    /// Manages user session data throughout the application
    /// Roles: Admin, Staff, Accountant, Agent
    /// </summary>
    public static class SessionManager
    {
        public static User CurrentUser { get; set; }
        public static bool IsLoggedIn => CurrentUser != null;

        // Role checks
        public static bool IsAdmin => CurrentUser?.Role == "Admin";
        public static bool IsStaff => CurrentUser?.Role == "Staff";
        public static bool IsAccountant => CurrentUser?.Role == "Accountant";
        public static bool IsAgent => CurrentUser?.Role == "Agent";

        // Permission checks
        public static bool CanManageUsers => IsAdmin;
        public static bool CanManageProducts => IsAdmin || IsStaff;
        public static bool CanManageOrders => IsAdmin || IsStaff;
        public static bool CanViewOrders => IsAdmin || IsStaff || IsAccountant;

        public static void ClearSession()
        {
            CurrentUser = null;
        }
    }
}
