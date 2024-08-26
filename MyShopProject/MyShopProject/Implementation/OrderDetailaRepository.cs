using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderDetailsRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        public void Update(OrderDetails orderDetails)
        {
           _context.OrderDetails.Update(orderDetails);
        }

    }
}
