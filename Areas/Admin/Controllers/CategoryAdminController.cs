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
    [Route("Admin/CategoryAdmin")]
	[Authorize]
	public class CategoryAdminController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public CategoryAdminController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var categoriesQuery = db.Categories.Where(c =>
                (string.IsNullOrEmpty(search) || c.CategoryName.Contains(search)) &&
                c.Status != "Deleted");

            int totalItems = categoriesQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var categories = categoriesQuery
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToList();
            var categoryModels = _mapper.Map<List<CategoryModels>>(categories);
            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(categoryModels);
        }

        [Route("AddCategory")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Route("AddCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(CategoryModels model)
        {
            if (ModelState.IsValid)
            {
                if (db.Categories.Any(c => c.CategoryName == model.CategoryName && c.Status != "Deleted"))
                {
                    ModelState.AddModelError("CategoryName", "The category name must be unique.");
                    return View(model);
                }

                var newCategory = _mapper.Map<Category>(model);
                db.Categories.Add(newCategory);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CategoryModels>(category);
            return View(model);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CategoryModels model)
        {
            if (id != model.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                if (category == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, category);
                category.UpdatedAt = DateTime.Now;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Status = "Deleted";
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
