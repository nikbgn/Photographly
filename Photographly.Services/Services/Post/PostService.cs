namespace Photographly.Services.Services.Post
{
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;

	using Photographly.Core.Models.Post;
	using Photographly.Infrastructure.Data;
	using Photographly.Services.Contracts;

	public class PostService : IPostService
	{

		private readonly PhotographlyDbContext _context;

		public PostService(PhotographlyDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Creates a new post
		/// </summary>
		/// <param name="model">Information about the post.</param>
		/// <param name="userId">The ID of the user who is creating the post.</param>

		public async Task CreatePostAsync(CreatePostViewModel model, string userId)
		{
			
		}
	}
}
