using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using Supermaket.Helpers;
using Supermaket.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Supermaket.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineSupermarketContext db;

        public HomeController(ILogger<HomeController> logger, OnlineSupermarketContext context)
        {
            _logger = logger;
            db = context;
        }


        public IActionResult Index(int? category, int page = 1)
        {
            var claimRole = HttpContext.User.Claims.SingleOrDefault(p => p.Type == ClaimTypes.Role);
            if (claimRole != null && claimRole.Value == "Admin")
            {
                return RedirectToAction("Index", "Admin"); 
            }

            int noOfRecord = 8;

			var products = db.Products
					 .Where(p => p.Status != "Deleted") 
					 .AsQueryable();


			if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }
            int totalRecords = products.Count();
            int noOfPage = (int)Math.Ceiling((double)totalRecords / noOfRecord);
            int noOfRecordSkip = (page - 1) * noOfRecord;
            ViewBag.page = page;
            ViewBag.noOfPage = noOfPage;
            ViewBag.category = category;
            var result = products
                .Select(p => new ProductModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Status = p.Status,
                    ViewCount = p.ViewCount,
                    ProductImage = p.ProductImage,
                    CategoryName = p.Category.CategoryName,
                    
                })
                .Skip(noOfRecordSkip)
                .Take(noOfRecord)
                .ToList();

            return View(result);
        }



        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
