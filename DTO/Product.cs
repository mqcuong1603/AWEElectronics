namespace AWEElectronics.DTO
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Specifications { get; set; } // JSON string
        public decimal Price { get; set; }
        public int StockLevel { get; set; }
        public bool IsPublished { get; set; }

        // Navigation properties for display
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
    }
}
