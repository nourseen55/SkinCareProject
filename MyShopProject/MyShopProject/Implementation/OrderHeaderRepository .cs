using Microsoft.EntityFrameworkCore;
using MyShopProject.Data;
using MyShopProject.Models;
using MyShopProject.Repositories;

namespace MyShopProject.Implementation
{
	public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public void Update(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
		}

		public void Updateorderstatus(int id, string orderstatus, string paymentstatus)
		{
			var orderheaderindb=_context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if (orderheaderindb != null) {
					orderheaderindb.OrderStatus = orderstatus;
					orderheaderindb.PaymentDate=DateTime.Now;
				if (paymentstatus != null) { 
					orderheaderindb.PaymentStatus = paymentstatus;
				
				}
			}
		}
	}
}
