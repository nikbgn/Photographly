namespace Photographly.Core.Models.Post
{
	using System.ComponentModel.DataAnnotations;

	public class PostsQueryModel
	{
		public int PostsPerPage { get; set; } = 8;
		[Display(Name = "Search")]
		public string SearchTerm { get; set; }
		public int TotalPostsCount { get; set; }
		public int CurrentPage { get; set; } = 1;
		public IEnumerable<PostServiceModel> Posts { get; set; }
	}
}
