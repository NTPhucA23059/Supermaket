using Supermaket.Data;
using System.ComponentModel.DataAnnotations;

namespace Supermaket.Models
{
    public class LoginModels
    {

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }


    }
    public class Register
    {

        [Required(ErrorMessage = "Account ID is required.")]
        [Display(Name = "Account ID")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        [Display(Name = "Username")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Contact Number")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
      //  [RegularExpression(@"^=.*[a-z](?=.[A-Z]).+$",ErrorMessage ="jsugdhs")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
        [Display(Name = "Role")]
        public string Role { get; set; } = "Customer"; // Giá trị mặc định

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "active"; // Giá trị mặc định

        [Required(ErrorMessage = "Created At is required.")]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Updated At is required.")]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "Profile Picture URL cannot exceed 100 characters.")]
        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; } 

        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        [Display(Name = "Credit Card Number")]
        public string? CreditCardNumber { get; set; }

        [StringLength(5, MinimumLength = 5, ErrorMessage = "Credit Card Expiry should be in MM/YY format.")]
        [Display(Name = "Credit Card Expiry")]
        public string? CreditCardExpiry { get; set; }


        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<UserProductHistory> UserProductHistories { get; set; } = new List<UserProductHistory>();

        public virtual ICollection<UserPromotion> UserPromotions { get; set; } = new List<UserPromotion>();
    }

	public class ForgotPasswordModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}

    public class ResetPasswordModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }


}

