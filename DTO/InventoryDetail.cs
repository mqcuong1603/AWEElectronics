namespace AWEElectronics.DTO
{
    public class InventoryDetail
    {
        public int ID { get; set; }
        public int TransID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal CostOrPrice { get; set; }

        // Navigation property
        public string ProductName { get; set; }
    }
}
