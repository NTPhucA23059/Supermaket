using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Supermaket.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("Admin/ProductAdmin")]
	[Authorize]
	public class ProductAdminController : Controller
	{
		private readonly OnlineSupermarketContext db;
		private readonly IMapper _mapper;
		public ProductAdminController(OnlineSupermarketContext context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}

        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var productsQuery = db.Products.Include(p => p.Category)
                                           .Include(p => p.Brand)
                                           .Where(p => (string.IsNullOrEmpty(search) || p.ProductName.Contains(search)) && p.Status != "Deleted");

            int totalItems = productsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var products = productsQuery
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToList();
            var productModels = _mapper.Map<List<ProductModels>>(products);
            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(productModels);
        }


        [Route("AddProduct")]
        public IActionResult AddProduct()
        {
           
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View();
        }

        [HttpPost]
        [Route("AddProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductModels model, IFormFile productImage)
        {
			ViewBag.Categories = db.Categories.Where(c => c.Status != "Deleted").ToList();
			ViewBag.Brands = db.Brands.Where(b => b.Status != "Deleted").ToList();

			if (ModelState.IsValid)
            {
                if (db.Products.Any(s => s.ProductName == model.ProductName && s.Status != "Deleted"))
                {
                    ModelState.AddModelError("ProductName", "The product name must be unique.");
                    return View(model);
                }

                var newProduct = _mapper.Map<Product>(model);

                if (productImage != null)
                {
                    var imagePath = MyUtil.UploadHinh(productImage, "Product");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        newProduct.ProductImage = imagePath;
                    }
                }

                db.Products.Add(newProduct);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }


        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var product = db.Products.Include(p => p.Category).Include(p => p.Brand).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ProductModels>(product);

            ViewBag.Categories = new SelectList(db.Categories.Where(c => c.Status != "Deleted"), "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.Brands = new SelectList(db.Brands.Where(b => b.Status != "Deleted"), "BrandId", "BrandName", product.BrandId);

            return View(model);
        }


        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductModels model, IFormFile productImage)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(db.Categories.Where(c => c.Status != "Deleted"), "CategoryId", "CategoryName", model.CategoryId);
            ViewBag.Brands = new SelectList(db.Brands.Where(b => b.Status != "Deleted"), "BrandId", "BrandName", model.BrandId);

            if (ModelState.IsValid)
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, product);

                product.UpdatedAt = DateTime.Now;

                if (productImage != null)
                {
                    var imagePath = MyUtil.UploadHinh(productImage, "Product");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        product.ProductImage = imagePath;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product); 
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Status = "Deleted";
            db.SaveChanges();

            return RedirectToAction("Index");
        }






    }
}
