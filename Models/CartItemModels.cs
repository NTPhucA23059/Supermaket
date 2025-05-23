using Supermaket.Data;

namespace Supermaket.Models
{
	public class CartItemModels
	{
		public int CartItemId { get; set; }
		public int? CartId { get; set; }
		public int? ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public decimal Total => Price * Quantity;

		public virtual Cart Cart { get; set; }
		public virtual Product Product { get; set; }



	}
}
