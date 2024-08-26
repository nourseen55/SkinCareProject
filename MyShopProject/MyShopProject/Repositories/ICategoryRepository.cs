using MyShopProject.Models;

namespace MyShopProject.Repositories
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		void Update(Category category);
	}
}
