using Supermaket.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supermaket.Areas.Admin.Models
{
    public class PromotionModels
    {
        public int PromotionId { get; set; }

        [Required(ErrorMessage = "Promotion code is required.")]
        [StringLength(10, ErrorMessage = "Promotion code cannot exceed 10 characters.")]
        [Display(Name = "Promotion Code")]
        public string PromotionCode { get; set; } = null!;

        [Required(ErrorMessage = "Promotion PromotionType is required.")]
        [StringLength(50, ErrorMessage = "Promotion type cannot exceed 50 characters.")]
        [Display(Name = "Promotion Type")]
        public string? PromotionType { get; set; }


        [Required(ErrorMessage = "Promotion PromotionDescription is required.")]
        [StringLength(500, ErrorMessage = "Promotion description cannot exceed 500 characters.")]
        [Display(Name = "Promotion Description")]
        public string? PromotionDescription { get; set; }

        [Required(ErrorMessage = "Promotion DiscountPercentage is required.")]
        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        [Display(Name = "Discount Percentage")]
        public decimal? DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Available";

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
