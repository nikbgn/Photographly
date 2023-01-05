namespace Photographly.Services.Contracts
{
	using Photographly.Core.Models.Post;

	public interface IPostService
	{
		/// <summary>
		/// Creates a new post
		/// </summary>
		/// <param name="model">Information about the post.</param>
		/// <param name="userId">The ID of the user who is creating the post.</param>

		public Task CreatePostAsync(CreatePostViewModel model, string userId);

		/// <summary>
		/// Gets user's posts.
		/// </summary>
		/// <param name="userId">User's ID</param>

		public Task<IEnumerable<PostServiceModel>> GetMyPostsAsync(string userId);

		/// <summary>
		/// Deletes a post
		/// </summary>
		/// <param name="postId">Post id</param>
		/// <returns></returns>

		public Task DeletePostAsync(Guid postId);

		/// <summary>
		/// Edits a post.
		/// </summary>
		/// <param name="model">Information about the post.</param>

		public Task EditPostAsync(CreatePostViewModel model);

		/// <summary>
		/// Gets a post by id.
		/// </summary>
		/// <param name="postId">Post's id.</param>

		public Task<PostServiceModel> GetPostAsync(Guid postId);

		/// <summary>
		/// Gets all posts.
		/// </summary>
		/// <param name="searchTerm">Search term.</param>
		/// <param name="currentPage">Current page.</param>
		/// <param name="postsPerPage">Posts per page.</param>

		public PostQueryServiceModel All(string searchTerm = null, int currentPage = 1, int postsPerPage = 1);
	}
}
