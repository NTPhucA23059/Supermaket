using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Supermaket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/BrandAdmin")]
	[Authorize]
	public class BrandAdminController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public BrandAdminController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var brandsQuery = db.Brands.Where(b =>
                (string.IsNullOrEmpty(search) || b.BrandName.Contains(search)) &&
                b.Status != "Deleted");

            int totalItems = brandsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var brands = brandsQuery
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToList();
            var brandModels = _mapper.Map<List<BrandModels>>(brands);
            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(brandModels);
        }


        [Route("AddBrand")]
        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        [Route("AddBrand")]
        [ValidateAntiForgeryToken]
        public IActionResult AddBrand(BrandModels model)
        {
            if (ModelState.IsValid)
            {
                if (db.Brands.Any(b => b.BrandName == model.BrandName && b.Status != "Deleted"))
                {
                    ModelState.AddModelError("BrandName", "The brand name must be unique.");
                    return View(model);
                }

                var newBrand = _mapper.Map<Brand>(model);
                db.Brands.Add(newBrand);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var brand = db.Brands.FirstOrDefault(b => b.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<BrandModels>(brand);
            return View(model);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BrandModels model)
        {
            if (id != model.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var brand = db.Brands.FirstOrDefault(b => b.BrandId == id);
                if (brand == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, brand);
                brand.UpdatedAt = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var brand = db.Brands.FirstOrDefault(b => b.BrandId == id);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var brand = db.Brands.FirstOrDefault(b => b.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            brand.Status = "Deleted";
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
