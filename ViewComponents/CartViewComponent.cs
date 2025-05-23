using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermaket.Models;
using Supermaket.Data;
using System.Security.Claims;
using Supermaket.Helpers;

namespace Supermaket.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly OnlineSupermarketContext db;

        public CartViewComponent(OnlineSupermarketContext context) => db = context;

    

        public IViewComponentResult Invoke()
        {
            var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

            int userId = 0;  
            if (claimCustomer != null)
            {
                int.TryParse(claimCustomer.Value, out userId);  
            }

            if (userId == 0)
            {
                return View("CartPanel", new
                {
                    Cart = new CartModels(),
                    Quantity = 0,
                    Total = 0
                });
            }

            if (userId == 0)
            {
                return View("CartPanel", new
                {
                    Cart = new CartModels(),
                    Quantity = 0,
                    Total = 0
                });
            }

            var cartItems = db.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.Cart.UserId == userId)
                .ToList();

            // Tính tổng số lượng và tổng giá trị của giỏ hàng
            var quantity = cartItems.Sum(ci => ci.Quantity);
            var total = cartItems.Sum(ci => ci.Price * ci.Quantity);

            var cartModel = new CartModels
            {
                CartId = 0,
                UserId = userId,
                Status = "Active",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return View("CartPanel", new
            {
                Cart = cartModel,
                Quantity = quantity,
                Total = total
            });
        }

    }

}
