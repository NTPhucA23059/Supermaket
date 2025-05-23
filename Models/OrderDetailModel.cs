using Supermaket.Data;
using Supermaket.Models;

namespace Supermaket.Models
{
	public class OrderDetailModel
	{
		public int OrderItemId { get; set; }
		public int OrderId { get; set; } 
		public int? ProductId { get; set; } 
		public int Quantity { get; set; }
		public decimal Price { get; set; }
        public decimal Total => Price * Quantity;
        public virtual Order Order { get; set; } 
		public virtual Product Product { get; set; } 
		public string Status { get; set; }
	}
}
