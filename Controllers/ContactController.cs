using Microsoft.AspNetCore.Mvc;

namespace Supermaket.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
