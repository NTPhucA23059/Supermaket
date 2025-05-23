using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Supermaket.Areas.Admin.Models
{
    public class CategoryModels
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Created date is required.")]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Display(Name = "Products")]
        public virtual ICollection<ProductModels> Products { get; set; } = new List<ProductModels>();
    }
}
