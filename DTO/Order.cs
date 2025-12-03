using System;
using System.Collections.Generic;

namespace AWEElectronics.DTO
{
    public class Order
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public int CustomerID { get; set; }
        public int? StaffCheckedID { get; set; }
        public int ShippingAddressID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; } // Pending, Processing, Shipped, Delivered, Cancelled
        public DateTime OrderDate { get; set; }

        // Navigation properties for display
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string StaffName { get; set; }
        public string ShippingAddress { get; set; }

        // Order details
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
