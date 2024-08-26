using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        public void Update(Category category)
        {
            var CategoryInDb = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (CategoryInDb != null)
            {
                CategoryInDb.Name = category.Name;
                CategoryInDb.Description = category.Description;
                CategoryInDb.CreatedAt = DateTime.Now;

                _context.Categories.Attach(CategoryInDb);
                _context.Entry(CategoryInDb).State = EntityState.Modified;
            }
        }

    }
}
