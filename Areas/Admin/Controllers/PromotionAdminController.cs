using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Supermaket.Areas.Admin.Repository;

namespace Supermaket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/PromotionAdmin")]
	[Authorize]
	public class PromotionAdminController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public PromotionAdminController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var promotionsQuery = db.Promotions
                                    .Where(p => (string.IsNullOrEmpty(search) || p.PromotionCode.Contains(search)) && p.Status != "Deleted");

            int totalItems = promotionsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var promotions = promotionsQuery
                            .Skip((page - 1) * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToList();
            var promotionModels = _mapper.Map<List<PromotionModels>>(promotions);

            foreach (var promotion in promotionModels)
            {
                if (promotion.EndDate < DateTime.Now)
                {
                    promotion.Status = "Expired";
                }
            }

            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(promotionModels);
        }

        [Route("AddPromotion")]
        public IActionResult AddPromotion()
        {
            return View();
        }

        [HttpPost]
        [Route("AddPromotion")]
        [ValidateAntiForgeryToken]
        public IActionResult AddPromotion(PromotionModels model)
        {
            if (ModelState.IsValid)
            {
                var existingPromotion = db.Promotions
            .FirstOrDefault(p => p.PromotionCode == model.PromotionCode && p.Status != "Deleted");

                if (existingPromotion != null)
                {
                    ModelState.AddModelError("PromotionCode", "The promotion code already exists and is not deleted.");
                    return View(model);
                }

                var newPromotion = _mapper.Map<Promotion>(model);

                if (newPromotion.EndDate < DateTime.Now)
                {
                    newPromotion.Status = "Expired"; 
                }

                db.Promotions.Add(newPromotion);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var promotion = db.Promotions.FirstOrDefault(p => p.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<PromotionModels>(promotion);

            if (promotion.EndDate < DateTime.Now)
            {
                model.Status = "Expired"; 
            }

            return View(model);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PromotionModels model)
        {
            if (id != model.PromotionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var promotion = db.Promotions.FirstOrDefault(p => p.PromotionId == id);
                if (promotion == null)
                {
                    return NotFound();
                }

                _mapper.Map(model, promotion);
              

                if (promotion.EndDate < DateTime.Now)
                {
                    promotion.Status = "Expired";
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
            var promotion = db.Promotions.FirstOrDefault(p => p.PromotionId == id);

            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var promotion = db.Promotions.FirstOrDefault(p => p.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }

            promotion.Status = "Deleted";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("SendEmailToAllCustomers")]
        [ValidateAntiForgeryToken]
        public IActionResult SendEmailToAllCustomers(int promotionId, string subject, string message)
        {
            var promotion = db.Promotions.FirstOrDefault(p => p.PromotionId == promotionId);
            if (promotion == null)
            {
                return NotFound();
            }
            var customers = db.Accounts.Where(a => a.Status != "Deleted").ToList();

            var emailSender = new EmailSender();

            foreach (var customer in customers)
            {
                var emailSubject = $"Promotion: {promotion.PromotionCode} is now available!";
                var emailMessage = $"Dear {customer.Username},\n\nWe are pleased to announce that the promotion '{promotion.PromotionCode}' is now available! Enjoy a discount of {promotion.DiscountPercentage}% on selected products. Don't miss out!\n\nBest regards,\nSupermarket Team";
                emailSender.SendEmailAsync(customer.Email, emailSubject, emailMessage);
            }

            TempData["Message"] = "Emails have been sent to all customers.";
            return RedirectToAction("Index");
        }


    }
}
