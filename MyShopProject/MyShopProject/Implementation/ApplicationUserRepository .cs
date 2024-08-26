using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
	{
		private readonly ApplicationDbContext _context;

		public ApplicationUserRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}



    }
}
