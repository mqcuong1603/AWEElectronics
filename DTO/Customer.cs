using System;

namespace AWEElectronics.DTO
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? DefaultShippingAddressID { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
