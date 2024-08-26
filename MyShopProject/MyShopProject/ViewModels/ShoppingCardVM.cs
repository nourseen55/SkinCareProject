using MyShopProject.Models;

namespace MyShopProject.ViewModels
{
    public class ShoppingCardVM
    {
        public IEnumerable<ShoppingCart> ShoppingCarts { get; set; }
        public decimal totalcarts { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
