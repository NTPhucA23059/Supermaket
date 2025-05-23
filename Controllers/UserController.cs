using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Supermaket.Models;
using Supermaket.Data;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Supermaket.Helpers;
using Supermaket.Areas.Admin.Repository;



namespace Supermaket.Controllers
{
    public class UserController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public UserController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }


        public IActionResult index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

   [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Register(Register model)
{
    if (ModelState.IsValid)
    {
        if (db.Accounts.Any(a => (a.Username == model.Username || a.Email == model.Email || a.ContactNumber == model.ContactNumber) && a.Status != "Deleted"))
        {
            ModelState.AddModelError("All", "The username, email, or contact number already exists.");
            return View(model); 
        }

        var randomKey = MyUtil.GenerateRamdomKey();

        var hashedPassword = DataEncryptionExtensions.ToSHA256Hash(model.Password, randomKey);

        var newAccount = new Account
        {
            Username = model.Username,
            Email = model.Email,
            ContactNumber = model.ContactNumber,
            RandomKey = randomKey, 
            Password = hashedPassword,
            Role = model.Role ?? "Customer",
            Status = model.Status ?? "active", 
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ProfilePicture = model.ProfilePicture ?? "Default.jpg", 
            Address = model.Address,
            CreditCardNumber = model.CreditCardNumber,
            CreditCardExpiry = model.CreditCardExpiry
        };

        db.Accounts.Add(newAccount);
        db.SaveChanges();

        return RedirectToAction("Login", "User");
    }

    return View(model); 
}





        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModels model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                var account = db.Accounts.SingleOrDefault(a => a.Username == model.Username);
                if (account == null)
                {
                    ModelState.AddModelError("loi", "Account not found.");
                }
                else
                {
                    if (account.Status == "Inactive")
                    {
                        ModelState.AddModelError("loi", "Your account is inactive. Please contact the administrator.");
                    }
                    else if (account.Status == "Deleted")
                    {
                        ModelState.AddModelError("loi", "Your account has been deleted.");
                    }
                    else
                    {
                        var hashedPassword = DataEncryptionExtensions.ToSHA256Hash(model.Password, account.RandomKey);

                        if (hashedPassword != account.Password)
                        {
                            ModelState.AddModelError("loi", "Incorrect login details.");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Name, account.Username),
                        new Claim(MySetting.CLAIM_USERID, account.AccountId.ToString()),
                        new Claim(ClaimTypes.Role, account.Role)
                    };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                            account.LastLogin = DateTime.Now;
                            db.SaveChanges();

                            if (account.Role == "Admin")
                            {
                                return RedirectToAction("Index", "Admin");
                            }

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }

            return View();
        }



        // Logout

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [Authorize]
        [Authorize]
        public IActionResult Profile()
        {
            var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

            if (claimCustomer == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = int.Parse(claimCustomer.Value);
            var account = db.Accounts.SingleOrDefault(a => a.AccountId == userId);

            if (account == null)
            {
                return RedirectToAction("Login", "User");
            }

            var profileViewModel = new Register
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Email = account.Email,
                ContactNumber = account.ContactNumber,
                Address = account.Address,
                Password=account.Password,
                ProfilePicture = account.ProfilePicture,
                CreditCardNumber = account.CreditCardNumber,
                CreditCardExpiry = account.CreditCardExpiry
            };

            return View(profileViewModel); 
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendResetPasswordLink(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var account = db.Accounts.SingleOrDefault(a => a.Email == model.Email);
                if (account == null)
                {
                    ModelState.AddModelError("", "The email does not exist.");
                    return View("ForgotPassword", model);
                }

                var resetCode = Guid.NewGuid().ToString();

                account.ResetPasswordCode = resetCode;
                account.ResetPasswordExpires = DateTime.Now.AddMinutes(30); 
                db.SaveChanges();

                var resetLink = Url.Action("ResetPassword", "User", new { code = resetCode }, Request.Scheme);
                var emailSubject = "Password Reset Request";
                var emailMessage = $"Click the following link to reset your password: {resetLink}";

                var emailSender = new EmailSender();
                await emailSender.SendEmailAsync(model.Email, emailSubject, emailMessage);

                TempData["SuccessMessage"] = "A reset code has been sent to your email.";
                return RedirectToAction("Login", "User");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid password reset code.");
            }

            var account = db.Accounts.SingleOrDefault(a => a.ResetPasswordCode == code);
            if (account == null || account.ResetPasswordExpires < DateTime.Now)
            {
                ModelState.AddModelError("", "The reset code is invalid or has expired.");
                return View();
            }

            var model = new ResetPasswordModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var account = db.Accounts.SingleOrDefault(a => a.ResetPasswordCode == model.Code);
                if (account == null || account.ResetPasswordExpires < DateTime.Now)
                {
                    ModelState.AddModelError("", "The reset code is invalid or has expired.");
                    return View(model);
                }

                if (model.NewPassword.Length < 6)
                {
                    ModelState.AddModelError("NewPassword", "Password must be at least 6 characters long.");
                    return View(model);
                }

                var randomKey = MyUtil.GenerateRamdomKey();

                var hashedPassword = DataEncryptionExtensions.ToSHA256Hash(model.NewPassword, randomKey);
                account.Password = hashedPassword;
                account.RandomKey = randomKey;
                account.ResetPasswordCode = null;
                account.ResetPasswordExpires = null; 
                db.SaveChanges();

                TempData["SuccessMessage"] = "Your password has been successfully changed.";
                return RedirectToAction("Login", "User");
            }

            return View(model);
        }

    }
}
