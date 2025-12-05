using System;

namespace AWEElectronics.DTO
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; } // JSON string
        public decimal Price { get; set; }
        public int StockLevel { get; set; }
        public bool IsPublished { get; set; }
        public string Status { get; set; } // Published, Draft, Discontinued
        public string ImageURL { get; set; }
        public decimal? Weight { get; set; }
        public string Warranty { get; set; }
        public string Brand { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties for display
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
  
        // Add these navigation property objects
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
    }
}
