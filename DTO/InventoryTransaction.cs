using System;

namespace AWEElectronics.DTO
{
    public class InventoryTransaction
    {
        public int TransID { get; set; }
        public int ProductID { get; set; }
        public string Type { get; set; } // IN, OUT, ADJUST
        public int Quantity { get; set; }
        public string ReferenceNumber { get; set; }
        public int PerformedBy { get; set; }
        public DateTime TransDate { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public string PerformedByName { get; set; }
    }
}
