namespace Photographly.Areas.Photography.Controllers
{
    using Microsoft.AspNetCore.Mvc;

	using Photographly.Core.Models.Post;
	using Photographly.Extensions;
	using Photographly.Services.Contracts;

	[Area("Photography")]
    public class HomeController : Controller
    {
		private readonly IPostService _postService;

		public HomeController(IPostService postService)
		{
			_postService = postService;
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyPosts()
        {
            return View();
        }

		[HttpGet]
		public IActionResult Add()
		{
			var model = new CreatePostViewModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(CreatePostViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var userId = User.Id();
				await _postService.CreatePostAsync(model, userId);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}

		}
	}
}
