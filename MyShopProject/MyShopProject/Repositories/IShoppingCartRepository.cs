using MyShopProject.Models;

namespace MyShopProject.Repositories
{
	public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
	{
		int IncreaseCount(ShoppingCart cart,int count);
		int DecreaseCount(ShoppingCart cart,int count);
    }
}
