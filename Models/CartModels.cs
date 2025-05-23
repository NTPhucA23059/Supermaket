namespace Supermaket.Models
{
	public class CartModels
	{
		public int CartId { get; set; }

		public int? UserId { get; set; }

		public string Status { get; set; } = null!;

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string CouponCode { get; set; }

	}
}
