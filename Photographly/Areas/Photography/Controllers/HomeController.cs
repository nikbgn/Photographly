namespace Photographly.Areas.Photography.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Photography")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult MyPosts()
        {
            return View();
        }
    }
}
