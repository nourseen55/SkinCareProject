using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        
        public void Update(Product product)
            {
                var productInDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
                if (productInDb != null)
                {
                    productInDb.Name = product.Name;
                    productInDb.Description = product.Description;
                    productInDb.Price = product.Price;
                    productInDb.Img = product.Img;
                    productInDb.CategoryId = product.CategoryId;

                    // Mark the entity as modified
                    _context.Products.Update(productInDb);
                }
            }
        

        }
    }
