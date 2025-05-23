using Supermaket.Data;

namespace Supermaket.Areas.Admin.Models
{
	public class OrderModels
	{
		public int OrderId { get; set; }

		public int? UserId { get; set; }

		public int? PromotionId { get; set; }

		public DateTime OrderDate { get; set; }

		public decimal TotalAmount { get; set; }

		public string? FullName { get; set; }

		public string? Phone { get; set; }

		public string ShippingAddress { get; set; } = null!;

		public string Status { get; set; } = null!;

		public string? Note { get; set; }

		public decimal? DiscountAmount { get; set; }

		public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

		public virtual ICollection<OrderItemModels> OrderItems { get; set; } = new List<OrderItemModels>();

		public virtual Promotion? Promotion { get; set; }

		public virtual Account? User { get; set; }
	}

	public class OrderItemModels
	{
		public int OrderItemId { get; set; }

		public int? OrderId { get; set; }

		public int? ProductId { get; set; }

		public int Quantity { get; set; }

		public string Status { get; set; } = null!;

		public decimal Price { get; set; }

		public virtual Order? Order { get; set; }

		public virtual Product? Product { get; set; }
	}
}
