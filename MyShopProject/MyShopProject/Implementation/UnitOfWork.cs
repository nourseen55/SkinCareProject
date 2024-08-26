using MyShopProject.Data;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			CategoryRepository = new CategoryRepository(context);
			Product = new ProductRepository(context);
			ShoppingCart = new ShoppingCartRepository(context);
			OrderHeader = new OrderHeaderRepository(context);
			OrderDetails = new OrderDetailsRepository(context);
			ApplicationUser = new ApplicationUserRepository(context);
        }
		

        public int Complete()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
