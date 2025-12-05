using AWEElectronics.DTO;
using System.Collections.Generic;

namespace Web.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string SearchKeyword { get; set; }
        public string SortBy { get; set; }
    }
}
