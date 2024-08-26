using MyShopProject.Models;

namespace MyShopProject.Repositories
{
	public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
	{
		void Update(OrderHeader orderHeader);
		void Updateorderstatus(int id ,string orderstatus,string paymentstatus);
	}
}
