using MyShopProject.Models;

namespace MyShopProject.Repositories
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		void Update(Product pro);
	}
}
