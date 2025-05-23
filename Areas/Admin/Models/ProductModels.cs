using Supermaket.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supermaket.Areas.Admin.Models
{
    public class ProductModels
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be a non-negative integer.")]
        [Display(Name = "Quantity in Stock")]
        public int QuantityInStock { get; set; }


        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [Display(Name = "Brand")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Required(ErrorMessage = "Product status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "View count must be a non-negative integer.")]
        [Display(Name = "View Count")]
        public int ViewCount { get; set; }

        [StringLength(255, ErrorMessage = "Product image URL cannot exceed 255 characters.")]
        [Display(Name = "Product Image")]
        public string? ProductImage { get; set; }

        [StringLength(500, ErrorMessage = "Additional images URL cannot exceed 500 characters.")]
        [Display(Name = "Additional Images")]
        public string? AdditionalImages { get; set; }

        [Display(Name = "Discount")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        [Required(ErrorMessage = "Discount is required.")]
        public decimal? Discount { get; set; }


        // These are navigation properties, typically used for relationships.
        [Display(Name = "Brand")]
        public virtual Brand? Brand { get; set; }

        [Display(Name = "Category")]


        public virtual Category? Category { get; set; }

        [Display(Name = "Cart Items")]
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        [Display(Name = "Order Items")]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
