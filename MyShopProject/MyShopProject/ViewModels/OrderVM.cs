using MyShopProject.Models;

namespace MyShopProject.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails>OrderDetails { get; set; }
    }
}
