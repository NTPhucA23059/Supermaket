using Supermaket.Data;

namespace Supermaket.Models
{
	public class CategoryModel
	{
		public int CategoryId { get; set; }

		public string CategoryName { get; set; } = null!;

		public string? Description { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string Status { get; set; } = null!;

		public int Quantity { get; set; }
		public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
	}
}
