using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using Supermaket.Models;

namespace Supermaket.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly OnlineSupermarketContext db;
        public CategoryViewComponent(OnlineSupermarketContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.Categories
				.Where(lo => lo.Status != "Deleted")
				.Select(lo => new CategoryModel
            {
                CategoryId = lo.CategoryId,
                CategoryName = lo.CategoryName,
                CreatedAt = lo.CreatedAt,
                UpdatedAt = lo.UpdatedAt,
                Status = lo.Status,
                Description = lo.Description,
                Quantity=lo.Products.Count
               
            }).OrderBy(p => p.CategoryName);
            return View(data);
        }
    }
}
    
