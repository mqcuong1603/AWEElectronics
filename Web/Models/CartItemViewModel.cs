using AWEElectronics.DTO;

namespace Web.Models
{
    public class CartItemViewModel
    {
        public CartItem CartItem { get; set; }
        public Product Product { get; set; }
        public decimal LineTotal => Product?.Price ?? 0 * (CartItem?.Quantity ?? 0);
    }
}
