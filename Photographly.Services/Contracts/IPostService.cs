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

		public Task<ViewPostViewModel> GetPostAsync(Guid postId);

		/// <summary>
		/// Gets all posts.
		/// </summary>
		/// <param name="searchTerm">Search term.</param>
		/// <param name="currentPage">Current page.</param>
		/// <param name="postsPerPage">Posts per page.</param>

		public PostQueryServiceModel All(string searchTerm = null, int currentPage = 1, int postsPerPage = 1);



		/// <summary>
		/// Allows a user to like a post.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public Task AddLikeToPost(string userId, Guid postId);

		/// <summary>
		/// Allows a user to remove his like from a post.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public Task RemoveLikeFromPost(string userId, Guid postId);

		/// <summary>
		/// Checks if the post is liked by a specific user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public Task<bool> PostIsLikedByUser(string userId, Guid postId);

		/// <summary>
		/// Adds a comment to a post.
		/// </summary>
		/// <param name="model">Information about the comment.</param>
		/// <param name="authorId">User Id of the author of the comment.</param>
		/// <param name="postId">Post id.</param>

		public Task AddComment(PostCommentModel model, string authorId, Guid postId);
	}
}
