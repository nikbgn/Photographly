﻿namespace Photographly.Services.Contracts
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
	}
}
