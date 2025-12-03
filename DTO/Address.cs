namespace AWEElectronics.DTO
{
    public class Address
    {
        public int AddressID { get; set; }
        public int CustomerID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Type { get; set; } // Shipping, Billing, Both

        // Navigation property
        public string CustomerName { get; set; }
    }
}
