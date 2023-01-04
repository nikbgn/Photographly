namespace Photographly.Areas.Photography.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	using Photographly.Core.Models.Post;
	using Photographly.Extensions;
	using Photographly.Services.Contracts;
	using Photographly.Services.Services.CheckerService;

	[Area("Photography")]
    public class HomeController : Controller
    {
		private readonly IPostService _postService;
		private readonly ICheckerService _checkerService;
		private readonly ILogger<HomeController> _logger;

		public HomeController(IPostService postService, ICheckerService checkerService, ILogger<HomeController> logger)
		{
			_postService = postService;
			_checkerService = checkerService;
			_logger = logger;
		}

        public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> MyPosts()
		{
			var userId = User.Id();
			var posts = await _postService.GetMyPostsAsync(userId);
			return View(posts);
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

		[Route("/Photography/Home/Delete/{postId}")]
		public async Task<IActionResult> Delete(Guid postId)
		{
			try
			{
				bool verify = await _checkerService.CheckIfUserIsPostAuthor(User.Id(), postId);
				if (!verify)
				{
					throw new Exception("User cannot delete other user's posts.");
				}
				await _postService.DeletePostAsync(postId);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex) { _logger.LogError($"ERROR MESSAGE: {ex.Message}"); return BadRequest(); }
		}

	}
}
