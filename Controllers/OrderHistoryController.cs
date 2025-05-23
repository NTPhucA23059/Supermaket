using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Supermaket.Models;

namespace Supermaket.Areas.Admin.Controllers
{

    [Authorize]
    public class OrderHistoryController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public OrderHistoryController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        public IActionResult Index(string status, int page = 1)
        {
            int noOfRecord = 6;

            var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);
            if (claimCustomer == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = int.Parse(claimCustomer.Value);

            var ordersQuery = db.Orders
                                .Where(o => o.UserId == userId && o.Status != "Deleted" && o.Status != "DeletedUser")
                                .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.Product)
                                .Include(o => o.Bills)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                ordersQuery = ordersQuery.Where(o => o.Status == status);
            }

            int totalRecords = ordersQuery.Count();
            int noOfPage = (int)Math.Ceiling((double)totalRecords / noOfRecord);
            int noOfRecordSkip = (page - 1) * noOfRecord;

            ViewBag.page = page;
            ViewBag.noOfPage = noOfPage;
            ViewBag.status = status;

            var orders = ordersQuery
                .Skip(noOfRecordSkip)
                .Take(noOfRecord)
                .ToList();

            var orderModels = _mapper.Map<List<OrderModels>>(orders);

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
        [Authorize]
        public IActionResult DeleteOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = "DeletedUser";

            db.SaveChanges();

            return RedirectToAction("Index", "OrderHistory");
        }




        [HttpPost]
        [Authorize]
        public IActionResult ConfirmDelivery(int id)
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            var order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.Status == "paid")
            {
                order.Status = "delivered";
                db.SaveChanges();

                var bill = new Bill
                {
                    OrderId = order.OrderId,
                    BillDate = DateTime.Now,
                    TotalAmount = order.TotalAmount,
                    PaymentMethod = "Cash on Delivery",  
                    PaymentStatus = "Success",
                    TransactionId = "No",  
                    UserName = username 
                };

                db.Bills.Add(bill);
                db.SaveChanges();
            }
            else
            {
                return BadRequest("This order cannot be confirmed for delivery.");
            }

            return RedirectToAction("Index", "OrderHistory");
        }





        [HttpPost]
        [Authorize]
        public IActionResult CancelOrder(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }

         
            if (order.Status != "delivered" && order.Status != "cancelled")
            {
                order.Status = "cancelled";  
                db.SaveChanges();
            }

            return RedirectToAction("Index", "OrderHistory");
        }







        [HttpGet]    
        public IActionResult Feedback(int orderItemId)
        {
            var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);
            if (claimCustomer == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = int.Parse(claimCustomer.Value);
            var orderItem = db.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Bills)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Brand)
                .FirstOrDefault(oi => oi.OrderItemId == orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            var customer = db.Accounts.SingleOrDefault(ac => ac.AccountId == userId);
            var username = customer?.Username ?? "";
            var productName = orderItem?.Product?.ProductName ?? "";

            var relatedProducts = db.Products
                .Where(p => p.CategoryId == orderItem.Product.CategoryId && p.ProductName != productName)
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
                .Where(r => r.ProductName == productName && r.UserName == username)
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
                ProductName = orderItem.Product?.ProductName,
                Price = orderItem.Product?.Price ?? 0,
                QuantityInStock = orderItem.Product?.QuantityInStock ?? 0,
                Description = orderItem.Product?.Description,
                ProductImage = orderItem.Product?.ProductImage,
                CategoryName = orderItem.Product?.Category?.CategoryName,
                BrandName = orderItem.Product?.Brand?.BrandName,
                RelatedProducts = relatedProducts,
                OrderItemId = orderItem.OrderItemId,
                AverageRating = averageRating,
                Reviews = reviews
            };

            return View(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SubmitReview(int orderItemId, string reviewText, int rating)
        {
            var orderItem = db.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Bills)
                .Include(oi => oi.Product)
                .FirstOrDefault(oi => oi.OrderItemId == orderItemId);

            if (orderItem == null)
            {
                return NotFound();
            }

            var username = orderItem.Order.FullName;
            var billId = orderItem.Order?.Bills?.SingleOrDefault()?.BillId;

            if (billId == null)
            {
                return BadRequest("BillId not found.");
            }

            var existingReview = db.ProductReviews
                .FirstOrDefault(pr => pr.UserName == username && pr.BillId == billId);

            if (existingReview != null)
            {
                existingReview.ReviewText = reviewText;
                existingReview.Rating = rating;
                existingReview.ReviewDate = DateTime.Now;
                existingReview.ProductName = orderItem.Product?.ProductName;
            }
            else
            {
                var productReview = new ProductReview
                {
                    ReviewText = reviewText,
                    Rating = rating,
                    ReviewDate = DateTime.Now,
                    BillId = billId.Value,
                    UserName = username,
                    ProductName = orderItem.Product?.ProductName
                };

                db.ProductReviews.Add(productReview);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}