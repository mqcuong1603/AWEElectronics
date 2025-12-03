using System;

namespace AWEElectronics.DTO
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }  // Admin, Staff, Accountant, Agent
        public string Status { get; set; } // Active, Inactive, Locked
        public DateTime CreatedDate { get; set; }
    }
}
