namespace Photographly.Services.Services.Post
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;

	using Photographly.Core.Models.Post;
	using Photographly.Infrastructure.Data;
	using Photographly.Infrastructure.Data.Models;
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

			var author =
				await _context.Users
				.Include(u => u.UsersPosts)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (author == null)
			{
				throw new ArgumentException("Invalid user id!");
			}

			var newPost = new Post()
			{
				Title = model.Title,
				Description = model.Description,
				LikesCount = model.LikesCount,
				CreatedOn = DateTime.Now
			};

			if (model.PostImage != null)
			{

				string[] acceptedExtensions = { ".png", ".jpg", ".jpeg" };

				if (!acceptedExtensions.Contains(Path.GetExtension(model.PostImage.FileName)))
				{
					throw new FormatException("Invalid image format!");
				}

				using MemoryStream ms = new MemoryStream();
				await model.PostImage.CopyToAsync(ms);

				if (ms.Length > 2097152)
				{
					throw new Exception("The size of the image you tried to upload is too large!");
				}

				newPost.PostImage = ms.ToArray();
			}
			else
			{
				throw new Exception("No image found!...");
			}

			author.UsersPosts.Add(new UserPost()
			{
				User = author,
				UserId = author.Id,
				Post = newPost,
				PostId = newPost.Id
			});

			await _context.SaveChangesAsync();


		}

		/// <summary>
		/// Gets user's posts.
		/// </summary>
		/// <param name="userId">User's ID</param>

		public async Task<IEnumerable<PostServiceModel>> GetMyPostsAsync(string userId)
		{

			var userPosts = await _context.Users
				.Where(u => u.Id == userId)
				.Include(u => u.UsersPosts)
				.ThenInclude(p => p.Post)
				.FirstOrDefaultAsync();

			if (userPosts == null)
			{
				throw new ArgumentException("Invalid user id!");
			}

			try
			{
				return userPosts.UsersPosts
					.Select(p => new PostServiceModel
					{
						Id = p.PostId,
						Title = p.Post.Title,
						Description = p.Post.Description,
						LikesCount = p.Post.LikesCount,
						CreatedOn = p.Post.CreatedOn,
						PostImage = p.Post.PostImage
					}).ToList();
			}
			catch (Exception)
			{
				throw new ApplicationException("Something went wrong in getting user's posts.");
			}
		}
	}
}
