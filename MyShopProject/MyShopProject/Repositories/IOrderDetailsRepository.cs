using MyShopProject.Models;

namespace MyShopProject.Repositories
{
	public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
	{
		void Update(OrderDetails orderDetails);
	}
}
