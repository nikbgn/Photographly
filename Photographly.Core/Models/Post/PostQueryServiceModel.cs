namespace Photographly.Core.Models.Post
{
	public class PostQueryServiceModel
	{
		public int TotalPostsCount { get; set; }

		public IEnumerable<PostServiceModel> Posts { get; set; } = new List<PostServiceModel>();
	}
}
