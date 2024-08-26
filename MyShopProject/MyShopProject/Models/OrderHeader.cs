using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyShopProject.Models
{
	public class OrderHeader
	{
		public int Id { get; set; }
		public string ApplicationUserId { get; set; }
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
		public DateTime?OrderTime { get; set; }
		public DateTime? PaymentDate { get; set; }
		public DateTime? ShippingTime { get; set; }
		public decimal TotalPrice { get; set; }
		public string? OrderStatus { get; set; }
		public string?PaymentStatus { get; set; }
		public string? TrackingNumber { get; set; }
		public string? Carrier { get; set; }
		public string? PaymentIntentId { get; set; }
		public string? SessionId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }







    }
}
