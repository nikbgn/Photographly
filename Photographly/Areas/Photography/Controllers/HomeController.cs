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


		[HttpGet]
		[Route("/Photography/Home/EditPost/{postId}")]
		public async Task<IActionResult> EditPost(Guid postId)
		{
			var verifyUser = await _checkerService.CheckIfUserIsPostAuthor(User.Id(), postId);

			if (!verifyUser)
			{
				return View("You do not have permissions to access this page!");
			}

			var getPost = await _postService.GetPostAsync(postId);
			var recipeToEdit = new CreatePostViewModel()
			{
				Id = getPost.Id,
				Title = getPost.Title,
				Description = getPost.Description,
				CreatedOn = getPost.CreatedOn,
				LikesCount = getPost.LikesCount
			};
			return View(recipeToEdit);
		}

		[HttpPost]
		public async Task<IActionResult> EditPost(CreatePostViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}
				await _postService.EditPostAsync(model);
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
