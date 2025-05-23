using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace Supermaket.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/OrderAdmin")]
	[Authorize]
	public class OrderAdminController : Controller
	{
		private readonly OnlineSupermarketContext db;
		private readonly IMapper _mapper;

		public OrderAdminController(OnlineSupermarketContext context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}

		[Route("Index")]
		public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
		{
			var ordersQuery = db.Orders
								.Where(o => (string.IsNullOrEmpty(search) || o.FullName.Contains(search) || o.ShippingAddress.Contains(search))
										   && o.Status != "Deleted");

			int totalItems = ordersQuery.Count();
			int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

			var orders = ordersQuery
						 .Skip((page - 1) * itemsPerPage)
						 .Take(itemsPerPage)
						 .ToList();

			var orderModels = _mapper.Map<List<OrderModels>>(orders);

			ViewBag.page = page;
			ViewBag.noOfPage = totalPages;
			ViewBag.search = search;

			return View(orderModels);
		}
        [Route("Detail/{id}")]
        public IActionResult ViewDetail(int id)
        {
            var order = db.Orders
                          .Include(o => o.OrderItems) 
                          .ThenInclude(oi => oi.Product) 
                          .Include(o => o.Bills) 
                          .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<OrderModels>(order);
            return View(model);
        }

        [HttpPost]
        [Route("UpdateStatus/{id}")]
        public IActionResult UpdateStatus(int id, string Status)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = Status;  
         
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var order = db.Orders
                          .Include(o => o.OrderItems)  
                          .ThenInclude(oi => oi.Product) 
                          .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order); 
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = "Deleted";

            db.SaveChanges();

            return RedirectToAction("Index");  
        }




    }
}
