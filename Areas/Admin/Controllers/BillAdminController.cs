using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Supermaket.Areas.Admin.Models;
using Supermaket.Data;


namespace Supermaket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/BillAdmin")]
    [Authorize]
    public class BillAdminController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public BillAdminController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var billsQuery = db.Bills
                     .Where(b => (string.IsNullOrEmpty(search) || b.UserName.Contains(search) || b.PaymentMethod.Contains(search)) && b.PaymentStatus != "Deleted");

            int totalItems = billsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var bills = billsQuery
                        .Skip((page - 1) * itemsPerPage)
                        .Take(itemsPerPage)
                        .ToList();

            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(bills);
        }

        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var bill = db.Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }
        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var bill = db.Bills.FirstOrDefault(b => b.BillId == id);
            if (bill == null)
            {
                return NotFound();
            }

            bill.PaymentStatus = "Deleted";
            bill.BillDate = DateTime.Now;

            db.SaveChanges();

            TempData["Message"] = "The payment status has been updated to 'Deleted'.";
            return RedirectToAction("Index");
        }



        //[HttpGet]
        //[Route("PrintBill/{id}")]
        //public IActionResult PrintBill(int id)
        //{
        //    var bill = db.Bills.FirstOrDefault(b => b.BillId == id);
        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }
        //    var model = new BillModels
        //    {
        //        BillId = bill.BillId,
        //        UserName = bill.UserName,
        //        BillDate = bill.BillDate,
        //        TotalAmount = bill.TotalAmount,
        //        PaymentMethod = bill.PaymentMethod,
        //        PaymentStatus = bill.PaymentStatus
        //    };
        //    return new ViewAsPdf("PrintBill", model)
        //    {
        //        FileName = $"Bill_{bill.BillId}.pdf"
        //    };
        //}


    }
}
