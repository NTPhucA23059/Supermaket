using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using Supermaket.Models;
using System.Linq;

namespace Supermaket.ViewComponents
{
    public class HomeViewComponent : ViewComponent
    {
        private readonly OnlineSupermarketContext db;

        public HomeViewComponent(OnlineSupermarketContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
          
            var categories = db.Categories.Select(lo => new CategoryModel
            {
                CategoryId = lo.CategoryId,
                CategoryName = lo.CategoryName,
                Quantity = lo.Products.Count
            }).OrderBy(p => p.CategoryName).ToList();

            return View(categories); 
        }
    }
}
