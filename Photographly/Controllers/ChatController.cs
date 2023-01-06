namespace Photographly.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ChatController : Controller
	{
		public IActionResult Index()
		{
			TempData["currUser"] = User.Identity!.Name;

            return View();
		}
	}
}
