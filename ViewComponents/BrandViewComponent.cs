using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using Supermaket.Models;

namespace Supermaket.ViewComponents
{
	public class BrandViewComponent:ViewComponent
	{
		private readonly OnlineSupermarketContext db;
		public BrandViewComponent(OnlineSupermarketContext context) => db = context;
		public IViewComponentResult Invoke()
		{
			var data = db.Brands
				.Where(b => b.Status != "Deleted")
				.Select(lo => new BrandModel
			{
				BrandId = lo.BrandId,
				BrandName = lo.BrandName,
				CreatedAt = lo.CreatedAt,
				UpdatedAt = lo.UpdatedAt,
				Status = lo.Status,
				Description = lo.Description,
				Quantity = lo.Products.Count

			}).OrderBy(p => p.BrandName);
			return View(data);
		}
	}
}

