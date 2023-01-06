namespace Photographly.Controllers
{
	using System.Diagnostics;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	using Photographly.Core.Models.Post;
	using Photographly.Extensions;
	using Photographly.Models;
	using Photographly.Services.Contracts;

	public class HomeController : Controller
	{
		private readonly IPostService _postService;
		private readonly ILogger<HomeController> _logger;

		public HomeController(IPostService postService, ILogger<HomeController> logger)
		{
			_postService = postService;
			_logger = logger;
		}

		[Authorize]
		[HttpGet]
		public IActionResult Index([FromQuery] PostsQueryModel query)
		{
			var posts = _postService.All(
					query.SearchTerm,
					query.CurrentPage,
					query.PostsPerPage);

			query.TotalPostsCount = posts.TotalPostsCount;
			query.Posts = posts.Posts.OrderByDescending(p => p.CreatedOn);

			return View(query);
		}

		[Authorize]
		[HttpGet]
		[Route("/Home/LikePost/{postId}")]
		public async Task<IActionResult> LikePost(Guid postId)
		{
			bool verifyNotLikedYet = await _postService.PostIsLikedByUser(User.Id(), postId);
			//If user clicks like on a post that is already liked, we accept that the user would like to remove his like.

			if (!verifyNotLikedYet)
			{
				await _postService.AddLikeToPost(User.Id(), postId);
			}
			else
			{
				await _postService.RemoveLikeFromPost(User.Id(), postId);
			}

			return RedirectToAction(nameof(ViewPost), new { postId });
		}

		[Authorize]
		[HttpGet]
		[Route("/Home/ViewPost/{postId}")]
		public async Task<IActionResult> ViewPost(Guid postId)
		{
			var post = await _postService.GetPostAsync(postId);
			return View(post);
		}

		[Authorize]
		public async Task<IActionResult> AddComment(PostCommentModel model, string authorId, Guid postId)
		{

			authorId = User.Id();
			await _postService.AddComment(model, authorId, postId);
			return RedirectToAction(nameof(ViewPost), new { postId });


		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}