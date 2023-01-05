namespace Photographly.Controllers
{
	using System.Diagnostics;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	using Photographly.Core.Models.Post;
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
			query.Posts = posts.Posts;

			return View(query);
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