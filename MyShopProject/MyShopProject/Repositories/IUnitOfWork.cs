namespace MyShopProject.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		int Complete();
		ICategoryRepository CategoryRepository { get; }
		IProductRepository Product { get; }
		IShoppingCartRepository ShoppingCart { get; }
		IOrderHeaderRepository OrderHeader { get; }
		IOrderDetailsRepository OrderDetails { get; }
		IApplicationUserRepository ApplicationUser {  get; }

    }
}
