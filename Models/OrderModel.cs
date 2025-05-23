using Supermaket.Data;

namespace Supermaket.Models
{
	public class OrderModel
	{
		public int OrderId { get; set; }
		public int UserId { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount { get; set; }
		public string? FullName { get; set; }
		public string? Phone { get; set; }
		public string ShippingAddress { get; set; }
		public string Status { get; set; }
		public string? Note { get; set; }
		public decimal ? DiscountAmount { get; set; }
		public virtual Account? User { get; set; }
		public virtual ICollection<OrderDetailModel> OrderItems { get; set; } = new List<OrderDetailModel>();

	}
}
