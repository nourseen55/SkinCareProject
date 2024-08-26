using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
	{
		private readonly ApplicationDbContext _context;

		public ShoppingCartRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        public int DecreaseCount(ShoppingCart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
            
        }

        public int IncreaseCount(ShoppingCart cart, int count)
        {
            cart.Count += count;
            return cart.Count;

        }
    }
}
