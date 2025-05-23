using Supermaket.Data;

namespace Supermaket.Models
{
	public class BrandModel
	{
		public int BrandId { get; set; }

		public string BrandName { get; set; } = null!;

		public string? Description { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string Status { get; set; } = null!;

		public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
		public int Quantity { get; set; }
	}
}
