namespace AWEElectronics.DTO
{
    public class OrderDetail
    {
        public int DetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        // Navigation properties
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        
        // Alias for view compatibility
        public string SKU => ProductSKU;
    }
}
