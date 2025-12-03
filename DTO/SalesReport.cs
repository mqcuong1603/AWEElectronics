using System;

namespace AWEElectronics.DTO
{
    public class SalesReport
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalItemsSold { get; set; }
    }

    public class ProductSalesReport
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
