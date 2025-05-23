using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using Supermaket.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Supermaket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly OnlineSupermarketContext _db;
        private readonly IMapper _mapper;

        public HomeController(OnlineSupermarketContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

		public IActionResult Index()
		{
			var accountCount = _db.Accounts.Count(); 
			var categoryCount = _db.Categories.Count(); 
			var brandCount = _db.Brands.Count();  
			var revenueThisMonth = _db.Bills
				.Where(b => b.BillDate.Month == DateTime.Today.Month && b.BillDate.Year == DateTime.Today.Year)
				.Sum(b => b.TotalAmount);  

			ViewBag.AccountsCreated = accountCount;
			ViewBag.CategoriesCount = categoryCount;
			ViewBag.BrandsCount = brandCount;
			ViewBag.MonthlyRevenue = revenueThisMonth;

			return View();
		}


		[HttpPost]
        [Route("GetRevenueByDate")]
        public IActionResult GetRevenueByDate(string filterdate)
        {
            var chartData = new List<StatisticalModel>();
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            if (filterdate == "today")
            {
                chartData = _db.Bills
                    .Where(b => b.BillDate.Date == today)
                    .GroupBy(b => b.BillDate.Date)
                    .Select(group => new StatisticalModel
                    {
                        Date = group.Key.ToString("yyyy-MM-dd"),
                        Revenue = group.Sum(b => b.TotalAmount)
                    })
                    .ToList();
            }
            else if (filterdate == "this_month")
            {
                for (DateTime date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
                {
                    var revenueForDay = _db.Bills
                        .Where(b => b.BillDate.Date == date)
                        .Sum(b => b.TotalAmount);

                    chartData.Add(new StatisticalModel
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        Revenue = revenueForDay 
                    });
                }
            }
            else if (filterdate == "all_year")
            {
                chartData = _db.Bills
                    .Where(b => b.BillDate.Year == today.Year)
                    .GroupBy(b => new { b.BillDate.Year, b.BillDate.Month })
                    .Select(group => new StatisticalModel
                    {
                        Date = $"{group.Key.Year}-{group.Key.Month:00}",
                        Revenue = group.Sum(b => b.TotalAmount)
                    })
                    .ToList();
            }

            return Json(chartData);
        }
        [HttpPost]
        [Route("GetRevenueByCategoryOrBrand")]
        public IActionResult GetRevenueByCategoryOrBrand(string filterBy)
        {
            IQueryable<object> salesData;

            if (filterBy == "category")
            {
                salesData = from b in _db.Bills
                            where b.PaymentStatus == "Success"
                            join oi in _db.OrderItems on b.OrderId equals oi.OrderId
                            join c in _db.Categories on oi.Product.CategoryId equals c.CategoryId
                            group oi by c.CategoryName into g
                            select new
                            {
                                CategoryBrand = g.Key,
                                TotalQuantitySold = g.Sum(oi => oi.Quantity)
                            };
            }
            else
            {
                salesData = from b in _db.Bills
                            where b.PaymentStatus == "Success" 
                            join oi in _db.OrderItems on b.OrderId equals oi.OrderId
                            join br in _db.Brands on oi.Product.BrandId equals br.BrandId
                            group oi by br.BrandName into g
                            select new
                            {
                                CategoryBrand = g.Key,
                                TotalQuantitySold = g.Sum(oi => oi.Quantity)
                            };
            }

            return Json(salesData.ToList());
        }
	}
}
