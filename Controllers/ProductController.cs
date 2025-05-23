using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermaket.Data;
using Supermaket.Models;

namespace Supermaket.Controllers
{
    public class ProductController : Controller
    {
        private readonly OnlineSupermarketContext db;
        public ProductController(OnlineSupermarketContext context)
        {
            db = context;
        }
        public IActionResult Index(int? category, int? brand, int page = 1)
        {
            int noOfRecord = 6;

            var products = db.Products

				 .Where(p => p.Status != "Deleted") 
                 .AsQueryable();
            if (category.HasValue)
            {
                products = products.Where(p => p.CategoryId == category.Value);
            }

            if (brand.HasValue)
            {
                products = products.Where(p => p.BrandId == brand.Value);
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
                    BrandName = p.Brand.BrandName,
                    Description = p.Description
                })
                .Skip(noOfRecordSkip)
                .Take(noOfRecord)
                .ToList();

            return View(result);
        }
        public IActionResult Search(string? query, int page = 1)
        {
            int noOfRecord = 6;

            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.ProductName.Contains(query));
            }
            int totalRecords = products.Count();
            int noOfPage = (int)Math.Ceiling((double)totalRecords / noOfRecord);
            int noOfRecordSkip = (page - 1) * noOfRecord;

            ViewBag.page = page;
            ViewBag.noOfPage = noOfPage;
            ViewBag.query = query; 

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
                    BrandName = p.Brand.BrandName
                })
                .Skip(noOfRecordSkip)
                .Take(noOfRecord)
                .ToList();

            return View(result);
        }
        public IActionResult Detail(int id)
        {
            var product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .SingleOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm với ID: {id}";
                return Redirect("/404");
            }

            product.ViewCount += 1;
            db.SaveChanges();
            var productName = product.ProductName;

            var relatedProducts = db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
                .Select(p => new ProductModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ProductImage = p.ProductImage ?? "",
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .Take(6)
                .ToList();

            var averageRating = db.ProductReviews
                .Where(r => r.ProductName == productName)
                .Average(r => (double?)r.Rating) ?? 0;

            var reviews = db.ProductReviews
                .Where(r => r.ProductName == productName)
                .Select(r => new ProductReviewModels
                {
                    ReviewDate = r.ReviewDate,
                    userName = r.UserName,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText
                })
                .ToList();

            var result = new ProductModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Description = product.Description,
                ProductImage = product.ProductImage,
                CategoryName = product.Category.CategoryName,
                BrandName = product.Brand.BrandName,
                RelatedProducts = relatedProducts,
                AverageRating = averageRating,
                Reviews = reviews,
                
            };

            return View(result);
        }


    }
}