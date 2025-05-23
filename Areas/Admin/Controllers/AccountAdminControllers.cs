using Microsoft.AspNetCore.Mvc;
using Supermaket.Data;
using AutoMapper;
using Supermaket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Supermaket.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace Supermaket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AccountAdmin")]
    [Authorize]
    public class AccountAdminController : Controller
    {
        private readonly OnlineSupermarketContext db;
        private readonly IMapper _mapper;

        public AccountAdminController(OnlineSupermarketContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [Route("Index")]
        public IActionResult Index(int page = 1, int itemsPerPage = 10, string search = "")
        {
            var accountsQuery = db.Accounts
                                  .Where(a => (string.IsNullOrEmpty(search) || a.Username.Contains(search) || a.Email.Contains(search))
                                             && a.Status != "Deleted");

            int totalItems = accountsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var accounts = accountsQuery
                           .Skip((page - 1) * itemsPerPage)
                           .Take(itemsPerPage)
                           .ToList();

            var accountModels = _mapper.Map<List<AccountModels>>(accounts);

            ViewBag.page = page;
            ViewBag.noOfPage = totalPages;
            ViewBag.search = search;

            return View(accountModels);
        }



        [Route("AddAccount")]
        public IActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        [Route("AddAccount")]
        [ValidateAntiForgeryToken]
        public IActionResult AddAccount(AccountModels model, IFormFile ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                if (db.Accounts.Any(a => (a.Username == model.Username || a.Email == model.Email || a.ContactNumber == model.ContactNumber) && a.Status != "Deleted"))
                {
                    ModelState.AddModelError("Username", "The username or email or ContactNumber already exists.");
                    return View(model);
                }

                var randomKey = MyUtil.GenerateRamdomKey();

                var hashedPassword = DataEncryptionExtensions.ToSHA256Hash(model.Password, randomKey);

                var newAccount = _mapper.Map<Account>(model);
                newAccount.CreatedAt = DateTime.Now;
                newAccount.UpdatedAt = DateTime.Now;
                newAccount.RandomKey = randomKey;
                newAccount.Password = hashedPassword;

                if (ProfilePicture != null)
                {
                    var imagePath = MyUtil.UploadHinh(ProfilePicture, "Account");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        newAccount.ProfilePicture = imagePath;
                    }
                }

                db.Accounts.Add(newAccount);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }



        // Edit Account - GET
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<AccountModels>(account);
            return View(model);
        }

        // Edit Account - POST
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AccountModels model)
        {
            if (id != model.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountId == id);
                if (account == null)
                {
                    return NotFound();
                }

                var randomKey = MyUtil.GenerateRamdomKey();

                var hashedPassword = DataEncryptionExtensions.ToSHA256Hash(model.Password, randomKey);
                _mapper.Map(model, account);
                account.UpdatedAt = DateTime.Now;
                account.ProfilePicture = account.ProfilePicture;
                account.RandomKey = randomKey;
                account.Password = hashedPassword;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // Delete Account - GET
        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        [Route("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var account = db.Accounts.FirstOrDefault(a => a.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            account.Status = "Deleted";
            account.UpdatedAt = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

     
        public IActionResult Profile()
        {
            var claimCustomer = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_USERID);

            if (claimCustomer == null)
            {
                return RedirectToAction("Login", "User");
            }

            var userId = int.Parse(claimCustomer.Value);
            var account = db.Accounts.FirstOrDefault(a => a.AccountId == userId && a.Status != "Deleted");
            if (account == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<AccountModels>(account);
            return View(model);
        }



    }
}
