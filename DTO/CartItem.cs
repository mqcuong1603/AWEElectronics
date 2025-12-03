using System;

namespace AWEElectronics.DTO
{
    public class CartItem
    {
        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }

        // Navigation properties
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductSKU { get; set; }
        public int ProductStockLevel { get; set; }
    }
}
