using Supermaket.Data;

namespace Supermaket.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int QuantityInStock { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Status { get; set; } = null!;

        public int ViewCount { get; set; }

        public string? ProductImage { get; set; }

        public string? AdditionalImages { get; set; }

		public string CategoryName { get; set; } 
		public string BrandName { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; } = null!;


		public int BrandId { get; set; }
		public Brand Brand { get; set; } = null!;

        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        public virtual ICollection<OrderDetailModel> OrderItems { get; set; } = new List<OrderDetailModel>();
        public double AverageRating { get; set; }
        // Sử dụng Reviews thay vì ProductReviews
        public List<ProductReviewModels> Reviews { get; set; } = new List<ProductReviewModels>();

        public List<ProductModel> RelatedProducts { get; set; } = new List<ProductModel>();

        public virtual ICollection<CartItemModels> CartItems { get; set; } = new List<CartItemModels>();
    }
}
