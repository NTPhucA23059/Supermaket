using Supermaket.Data;

namespace Supermaket.Models
{
	public class AccountModels
	{
		public int AccountId { get; set; }

		public string Username { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string? ContactNumber { get; set; }

		public string Password { get; set; } = null!;

		public string Role { get; set; } = null!;

		public string Status { get; set; } = null!;

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public DateTime? LastLogin { get; set; }

		public string? Address { get; set; }

		public string? ProfilePicture { get; set; }

		public string? CreditCardNumber { get; set; }

		public string? CreditCardExpiry { get; set; }

		public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();

		public virtual ICollection<UserProductHistory> UserProductHistories { get; set; } = new List<UserProductHistory>();

		public virtual ICollection<UserPromotion> UserPromotions { get; set; } = new List<UserPromotion>();
	}
}
