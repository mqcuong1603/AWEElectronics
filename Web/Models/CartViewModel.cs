using System.Collections.Generic;

namespace Web.Models
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public CartViewModel()
        {
            Items = new List<CartItemViewModel>();
        }
    }
}
