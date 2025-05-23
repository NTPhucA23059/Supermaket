using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supermaket.Areas.Admin.Repository;
using Supermaket.Data;
using Supermaket.Helpers;
using Supermaket.Models;
using System.Security.Claims;


namespace Supermaket.Controllers
{
	public class CartController : Controller
	{
		private readonly PaypalClient _paypalClient;
		private readonly OnlineSupermarketContext db;

		public CartController(OnlineSupermarketContext context, PaypalClient paypalClient)
		{
			_paypalClient = paypalClient;
			db = context;
		}

		[Authorize]
		public IActionResult Index()
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);

			var cartItems = db.CartItems
				.Include(ci => ci.Product)
				.Where(ci => ci.Cart.UserId == userId)
				.ToList();
			var cartItemModels = cartItems.Select(ci => new CartItemModels
			{
				CartItemId = ci.CartItemId,
				CartId = ci.CartId,
				ProductId = ci.ProductId,
				Quantity = ci.Quantity,
				Price = ci.Price,
				Image = ci.Product.ProductImage,
				Product = ci.Product
			}).ToList();

			return View(cartItemModels);
		}
		[Authorize]
		[HttpPost]
		public IActionResult UpdateQuantity(int cartItemId, int quantity)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);

			if (quantity <= 0)
			{
				return Json(new { totalPrice = 0, totalAmount = 0 });
			}

			var cartItem = db.CartItems
				.Include(ci => ci.Product)
				.FirstOrDefault(ci => ci.CartItemId == cartItemId);

			if (cartItem != null)
			{
				var productStock = cartItem.Product.QuantityInStock;
				if (quantity > productStock)
				{
					return Json(new { error = "Insufficient stock available." });
				}

				var oldQuantity = cartItem.Quantity;
				var quantityDifference = quantity - oldQuantity;

				cartItem.Quantity = quantity;
				cartItem.Product.QuantityInStock -= quantityDifference;

				db.SaveChanges();

				decimal total = cartItem.Price * cartItem.Quantity;

				var cartItems = db.CartItems
					.Include(ci => ci.Product)
					.Where(ci => ci.Cart.UserId == userId)
					.ToList();

				decimal totalAmount = cartItems.Sum(ci => ci.Price * ci.Quantity);

				return Json(new
				{
					totalPrice = total,
					totalAmount = totalAmount
				});
			}

			return Json(new { totalPrice = 0, totalAmount = 0 });
		}






		[HttpPost]
		public IActionResult AddToCart(int productId, int? quantity)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);
			quantity = quantity ?? 1;
			var product = db.Products.FirstOrDefault(p => p.ProductId == productId);

			if (product != null)
			{
				if (quantity > product.QuantityInStock)
				{
					TempData["ErrorMessage"] = $"Only {product.QuantityInStock} items are available in stock.";
					return RedirectToAction("Index", "Cart");
				}

				var cart = db.Carts.FirstOrDefault(c => c.UserId == userId);
				if (cart == null)
				{
					cart = new Cart { UserId = userId };
					db.Carts.Add(cart);
					db.SaveChanges();
				}

				var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == productId);

				if (cartItem != null)
				{
					cartItem.Quantity += quantity.Value;

					if (cartItem.Quantity > product.QuantityInStock)
					{
						cartItem.Quantity = product.QuantityInStock;
						TempData["ErrorMessage"] = $"Only {product.QuantityInStock} items are available in stock.";
					}
				}
				else
				{
					cartItem = new CartItem
					{
						CartId = cart.CartId,
						ProductId = productId,
						Quantity = quantity.Value,
						Price = product.Price
					};
					db.CartItems.Add(cartItem);
				}

				product.QuantityInStock -= quantity.Value;

				db.SaveChanges();

				var cartItems = db.CartItems
					.Include(ci => ci.Product)
					.Where(ci => ci.Cart.UserId == userId)
					.ToList();

				decimal totalAmount = cartItems.Sum(ci => ci.Price * ci.Quantity);

				return RedirectToAction("Index", "Cart");
			}

			return RedirectToAction("Index", "Home");
		}


		[HttpPost]
		[Authorize]
		public IActionResult RemoveCartItem(int cartItemId)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);

			var cartItem = db.CartItems
	.Include(ci => ci.Product)
	.FirstOrDefault(ci => ci.CartItemId == cartItemId && ci.Cart.UserId == userId);


			if (cartItem != null && cartItem.Product != null)
			{
				// Update product stock
				cartItem.Product.QuantityInStock += cartItem.Quantity;

				db.CartItems.Remove(cartItem);
				db.SaveChanges();
			}



			return RedirectToAction("Index", "Cart");
		}



		[HttpPost]
		[Authorize]
		public IActionResult CheckCoupon([FromBody] CartModels model)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);
			var coupon = db.Promotions
						  .FirstOrDefault(c => c.PromotionCode == model.CouponCode);

			if (coupon == null)
			{
				return Json(new { exists = false, message = "Coupon code does not exist." });
			}

			if (coupon.Status != "Available")
			{
				return Json(new { exists = false, message = "Coupon is not active." });
			}

			if (coupon.EndDate < DateTime.Now)
			{
				return Json(new { exists = false, message = "Coupon has expired." });
			}

			decimal discountPercentage = coupon.DiscountPercentage ?? 0;
			var cartItems = db.CartItems
				.Include(ci => ci.Product)
				.Where(ci => ci.Cart.UserId == userId)
				.ToList();

			decimal totalAmount = cartItems.Sum(ci => ci.Price * ci.Quantity);
			decimal discountAmount = Math.Round(discountPercentage / 100 * totalAmount, 2);
			decimal totalAfterDiscount = totalAmount - discountAmount;

			HttpContext.Session.SetString("DiscountAmount", discountAmount.ToString());
			HttpContext.Session.SetString("TotalAfterDiscount", totalAfterDiscount.ToString());
			HttpContext.Session.SetString("DiscountPercentage", discountPercentage.ToString());

			return Json(new
			{
				exists = true,
				discountPercentage = discountPercentage,
				discountAmount = discountAmount,
				totalAmount = totalAmount,
				totalAfterDiscount = totalAfterDiscount
			});
		}






		[HttpGet]
		[Authorize]
		public IActionResult Checkout()
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);
			var cartItems = db.CartItems
				.Include(ci => ci.Product)
				.Where(ci => ci.Cart.UserId == userId)
				.ToList();

			var cartItemModels = cartItems.Select(ci => new CartItemModels
			{
				CartItemId = ci.CartItemId,
				CartId = ci.CartId,
				ProductId = ci.ProductId,
				Quantity = ci.Quantity,
				Price = ci.Price,
				Image = ci.Product.ProductImage,
				Product = ci.Product
			}).ToList();

			if (!cartItems.Any())
			{
				return RedirectToAction("Index", "Cart");
			}
			ViewBag.PaypalClientdId = _paypalClient.ClientId;

			return View(cartItemModels);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Checkout(CheckoutModel model)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);
			var cartItemsModels = db.CartItems
								.Include(ci => ci.Product)
								.Where(ci => ci.Cart.UserId == userId)
								.Select(ci => new CartItemModels
								{
									CartItemId = ci.CartItemId,
									CartId = ci.CartId,
									ProductId = ci.ProductId,
									Quantity = ci.Quantity,
									Price = ci.Price,
									Image = ci.Product.ProductImage,
									Product = ci.Product,
								})
								.ToList();
			if (model.SameCustomerInformation == false)
			{
				if (string.IsNullOrEmpty(model.FullName) || string.IsNullOrEmpty(model.Address) || string.IsNullOrEmpty(model.Phone))
				{

					ViewBag.PaypalClientdId = _paypalClient.ClientId;
					ModelState.AddModelError("", "Please fill in all required fields.");
					return View(cartItemsModels);

				}
			}

			if (ModelState.IsValid)
			{
				var customer = db.Accounts.SingleOrDefault(ac => ac.AccountId == userId);
				if (customer == null)
				{

					ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
					return View();
				}

				var user = new Account();
				if (model.SameCustomerInformation)
				{
					user = db.Accounts.SingleOrDefault(kh => kh.AccountId == userId);
				}
				decimal t1 = cartItemsModels.Sum(ci => ci.Price * ci.Quantity);

				decimal discountAmount = Convert.ToDecimal(HttpContext.Session.GetString("DiscountAmount") ?? "0");
				decimal totalAfterDiscount = Convert.ToDecimal(HttpContext.Session.GetString("TotalAfterDiscount") ?? t1.ToString());

				decimal totalAmount = totalAfterDiscount > 0 ? totalAfterDiscount : 0m;


				var order = new Order
				{
					UserId = userId,
					OrderDate = DateTime.Now,
					Phone = model.Phone ?? user.ContactNumber,
					FullName = model.FullName ?? user.Username,
					TotalAmount = totalAmount,
					ShippingAddress = model.Address ?? user.Address,
					Status = "Processing",
					Note = model.Note ?? "Null",
					DiscountAmount = discountAmount,
				};

				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						db.Add(order);
						db.SaveChanges();

						var cartItems = db.CartItems
							.Include(ci => ci.Product)
							.Where(ci => ci.Cart.UserId == userId)
							.ToList();

						var orderItems = new List<OrderItem>();
						foreach (var item in cartItems)
						{
							orderItems.Add(new OrderItem
							{
								OrderId = order.OrderId,
								ProductId = item.ProductId,
								Quantity = item.Quantity,
								Price = item.Price
							});
						}

						db.AddRange(orderItems);
						db.SaveChanges();

						db.CartItems.RemoveRange(cartItems);
						db.SaveChanges();

						user = db.Accounts.SingleOrDefault(kh => kh.AccountId == userId);

						var emailSender = new EmailSender();
						string emailSubject = "Your Order is Successfully Placed";
						string emailMessage = $"Dear {model.FullName ?? user.Username},\n\nYour order has been successfully placed. Total Amount: {totalAmount}.\n\nThank you for shopping with us!";
						emailSender.SendEmailAsync(user.Email, emailSubject, emailMessage);

						transaction.Commit();

						return RedirectToAction("Success");
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						ModelState.AddModelError("", "Đã xảy ra lỗi khi đặt hàng: " + ex.Message);
						return View(cartItemsModels);
					}
				}
			}


			ViewBag.PaypalClientdId = _paypalClient.ClientId;
			return View(cartItemsModels);
		}


		[Authorize]
		public IActionResult Success()
		{
			return View();
		}
		[Authorize]
		public IActionResult PaymentSuccess(string orderCode)
		{
			ViewBag.OrderCode = orderCode;
			return View();
		}

		#region Paypal payment


		[Authorize]
		[HttpPost("/Cart/create-paypal-order")]
		public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
		{
			var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

			if (claimCustomer == null)
			{
				return RedirectToAction("Login", "User");
			}

			var userId = int.Parse(claimCustomer.Value);

			var cartItems = db.CartItems
			   .Include(ci => ci.Product)
			   .Where(ci => ci.Cart.UserId == userId)
			   .Select(ci => new CartItemModels
			   {
				   CartItemId = ci.CartItemId,
				   CartId = ci.CartId,
				   ProductId = ci.ProductId,
				   Quantity = ci.Quantity,
				   Price = ci.Price,
				   Image = ci.Product.ProductImage,
				   Product = ci.Product,
			   })
			   .ToList();

			decimal t1 = cartItems.Sum(ci => ci.Price * ci.Quantity);

			decimal discountAmount = Convert.ToDecimal(HttpContext.Session.GetString("DiscountAmount") ?? "0");
			decimal totalAfterDiscount = Convert.ToDecimal(HttpContext.Session.GetString("TotalAfterDiscount") ?? t1.ToString());

			decimal totalAmount = totalAfterDiscount > 0 ? totalAfterDiscount : 0m;
			var tongTien = totalAmount.ToString();
			var donViTienTe = "USD";
			var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}
		[Authorize]
		[HttpPost("/Cart/capture-paypal-order")]
		public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderID);

				var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

				if (claimCustomer == null)
				{
					return RedirectToAction("Login", "User");
				}

				var userId = int.Parse(claimCustomer.Value);
				var customer = db.Accounts.SingleOrDefault(ac => ac.AccountId == userId);

				if (customer == null)
				{
					ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
					return View();
				}

				var user = db.Accounts.SingleOrDefault(kh => kh.AccountId == userId);

				var cartItems = db.CartItems
					.Include(ci => ci.Product)
					.Where(ci => ci.Cart.UserId == userId)
					.ToList();


				decimal t1 = cartItems.Sum(ci => ci.Price * ci.Quantity);

				decimal discountAmount = Convert.ToDecimal(HttpContext.Session.GetString("DiscountAmount") ?? "0");
				decimal totalAfterDiscount = Convert.ToDecimal(HttpContext.Session.GetString("TotalAfterDiscount") ?? t1.ToString());


				decimal totalAmount = totalAfterDiscount > 0 ? totalAfterDiscount : 0m;

				var order = new Order
				{
					UserId = userId,
					OrderDate = DateTime.Now,
					Phone = user.ContactNumber,
					FullName = user.Username,
					DiscountAmount = discountAmount,
					TotalAmount = totalAmount,
					ShippingAddress = user.Address,
					Status = "Delivered",
					Note = "Null"
				};

				using (var transaction = db.Database.BeginTransaction())
				{
					try
					{
						db.Add(order);
						db.SaveChanges();

						var orderItems = new List<OrderItem>();
						foreach (var item in cartItems)
						{
							orderItems.Add(new OrderItem
							{
								OrderId = order.OrderId,
								ProductId = item.ProductId,
								Quantity = item.Quantity,
								Price = item.Price
							});
						}

						db.AddRange(orderItems);
						db.SaveChanges();


						db.CartItems.RemoveRange(cartItems);
						db.SaveChanges();

						var emailSender = new EmailSender();
						string emailSubject = "Your Order Payment was Successful";
						string emailMessage = $"Dear {user.Username},\n\nYour order payment was successful. Total Amount: {totalAmount}.\n\nThank you for shopping with us!";
						emailSender.SendEmailAsync(user.Email, emailSubject, emailMessage);
						var bill = new Bill
						{
							OrderId = order.OrderId,
							BillDate = DateTime.Now,
							TotalAmount = totalAmount,
							PaymentMethod = "PayPal",
							PaymentStatus = "Success",
							TransactionId = response?.purchase_units?.FirstOrDefault()?.payments?.captures?.FirstOrDefault()?.id,
							UserName = user.Username
						};

						db.Bills.Add(bill);
						await db.SaveChangesAsync();

						transaction.Commit();
						return RedirectToAction("Success");
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						ModelState.AddModelError("", "Đã xảy ra lỗi khi đặt hàng: " + ex.Message);
						return View();
					}
				}
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		#endregion

		public List<Product> compare => HttpContext.Session.Get<List<Product>>(MySetting.compare_key) ?? new List<Product>();

		public IActionResult Compare()
		{
			var compareList = HttpContext.Session.Get<List<Product>>(MySetting.compare_key) ?? new List<Product>();

			var compareModels = compareList.Select(p => new ProductModel
			{
				ProductId = p.ProductId,
				ProductName = p.ProductName,
				Price = p.Price,
				ProductImage = p.ProductImage,
				CategoryName = p.Category?.CategoryName,
				BrandName = p.Brand?.BrandName,
				Description = p.Description
			}).ToList();

			return View(compareModels);
		}





		[HttpPost]
		public IActionResult AddToCompare(int productId)
		{
			var product = db.Products
							.Include(p => p.Category)
							.Include(p => p.Brand)
							.FirstOrDefault(p => p.ProductId == productId);

			if (product != null)
			{
				var compareList = HttpContext.Session.Get<List<Product>>(MySetting.compare_key) ?? new List<Product>();

				if (!compareList.Any(p => p.ProductId == productId))
				{
					compareList.Add(product);
					HttpContext.Session.Set(MySetting.compare_key, compareList);
				}
			}

			return RedirectToAction("Compare");
		}



		[HttpPost]
		public IActionResult RemoveFromCompare(int productId)
		{
			var compareList = HttpContext.Session.Get<List<Product>>(MySetting.compare_key) ?? new List<Product>();

			var itemToRemove = compareList.FirstOrDefault(p => p.ProductId == productId);
			if (itemToRemove != null)
			{
				compareList.Remove(itemToRemove);
				HttpContext.Session.Set(MySetting.compare_key, compareList);
			}

			return RedirectToAction("Compare");
		}



	}
}
