namespace AWEElectronics.DTO
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryID { get; set; }
        public string Slug { get; set; }

        // Navigation property for display
        public string ParentCategoryName { get; set; }
    }
}
