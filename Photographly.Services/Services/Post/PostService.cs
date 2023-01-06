namespace Photographly.Services.Services.Post
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

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


		/// <summary>
		/// Deletes a post
		/// </summary>
		/// <param name="postId">Post id</param>
		/// <returns></returns>

		public async Task DeletePostAsync(Guid postId)
		{

			var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
			var postComments = _context.PostComments.Where(pc => pc.PostId == postId).ToList();
			if (post != null)
			{
				try
				{
					foreach (var comment in postComments)
					{
						_context.PostComments.Remove(comment);
					}
					_context.Posts.Remove(post);
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{

					throw new ApplicationException("Failed getting the post by id.", ex);
				}
			}
			else
			{
				throw new ArgumentException("Invalid post ID!");
			}
		}

		/// <summary>
		/// Edits a post.
		/// </summary>
		/// <param name="model">Information about the post.</param>

		public async Task EditPostAsync(CreatePostViewModel model)
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == model.Id);

			if (post == null)
			{
				throw new ArgumentException("The given post id was invalid.");
			}

			try
			{

				post.Id = model.Id;
				post.Title = model.Title;
				post.Description = model.Description;

				if (model.PostImage != null)
				{

					string[] acceptedExtensions = { ".png", ".jpg", ".jpeg" };

					if (!acceptedExtensions.Contains(Path.GetExtension(model.PostImage.FileName)))
					{
						throw new FormatException("Invalid image format.");
					}

					using MemoryStream ms = new MemoryStream();
					await model.PostImage.CopyToAsync(ms);

					if (ms.Length > 2097152)
					{
						throw new Exception("The size of the image you tried to upload is too large!");
					}

					post.PostImage = ms.ToArray();
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}

		/// <summary>
		/// Gets a post by id.
		/// </summary>
		/// <param name="postId">Post's id.</param>

		public async Task<ViewPostViewModel> GetPostAsync(Guid postId)
		{
			var post = await _context.Posts.Include(p => p.PostComments).ThenInclude(p => p.Author).FirstOrDefaultAsync(p => p.Id == postId);
			if (post == null)
			{
				throw new ArgumentException("The given post id was invalid.");
			}

			return new ViewPostViewModel()
			{
				Id = post.Id,
				Title = post.Title,
				Description = post.Description,
				PostImage = post.PostImage,
				CreatedOn = post.CreatedOn,
				LikesCount = post.LikesCount,
				PostComments = post.PostComments
			};
		}

		/// <summary>
		/// Gets all posts.
		/// </summary>
		/// <param name="searchTerm">Search term.</param>
		/// <param name="currentPage">Current page.</param>
		/// <param name="postsPerPage">Posts per page.</param>

		public PostQueryServiceModel All(string searchTerm = null, int currentPage = 1, int postsPerPage = 1)
		{
			var postsQuery = _context.Posts.AsQueryable();
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				postsQuery = postsQuery.Where(p => p.Title.ToLower().Contains(searchTerm.ToLower()));
			}

			var posts = postsQuery
                .OrderByDescending(p => p.CreatedOn)
                .Skip((currentPage - 1) * postsPerPage)
				.Take(postsPerPage)
				.Select(p => new PostServiceModel
				{
					Id = p.Id,
					Title = p.Title,
					Description = p.Description,
					CreatedOn = p.CreatedOn,
					PostImage = p.PostImage,
					LikesCount = p.LikesCount
				})
				.ToList();

			var totalPosts = postsQuery.Count();

			return new PostQueryServiceModel()
			{
				TotalPostsCount = totalPosts,
				Posts = posts.OrderByDescending(p => p.CreatedOn)
            };
		}


		/// <summary>
		/// Allows a user to like a post.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public async Task AddLikeToPost(string userId, Guid postId)
		{
			try
			{
				var user = await _context.Users
					.Where(u => u.Id == userId)
					.Include(u => u.UsersPosts)
					.ThenInclude(p => p.Post)
					.FirstOrDefaultAsync();

				var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

				if (user == null || post == null)
				{
					throw new ArgumentException("Invalid user id or post id.");
				}

				post.LikesCount += 1;

				var userLike = new UserLikes()
				{
					PostId = post.Id,
					Post = post,
					UserId = user.Id,
					User = user
				};

				user.UsersLikes.Add(userLike);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Allows a user to remove his like from a post.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public async Task RemoveLikeFromPost(string userId, Guid postId)
		{
			try
			{
				var user = await _context.Users
					.Where(u => u.Id == userId)
					.Include(u => u.UsersLikes)
					.Include(u => u.UsersPosts)
					.ThenInclude(p => p.Post)
					.FirstOrDefaultAsync();

				var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

				if (user == null || post == null)
				{
					throw new ArgumentException("Invalid user id or post id.");
				}

				post.LikesCount -= 1;

				var userLike = user.UsersLikes.FirstOrDefault(ul => ul.PostId == postId);

				user.UsersLikes.Remove(userLike!);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Checks if the post is liked by a specific user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="postId">Post Id.</param>

		public async Task<bool> PostIsLikedByUser(string userId, Guid postId)
		{
			var userLikes = await _context.Users
					.Where(u => u.Id == userId)
					.Include(u => u.UsersLikes)
					.ThenInclude(p => p.Post)
					.FirstOrDefaultAsync();

			var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

			if (userLikes == null || post == null)
			{
				throw new ArgumentException("Invalid user id or post id.");
			}

			if(userLikes.UsersLikes.Any(l => l.PostId == postId))
			{
				return true;
			}

			return false;


		}


		/// <summary>
		/// Adds a comment to a post.
		/// </summary>
		/// <param name="model">Information about the comment.</param>
		/// <param name="authorId">User Id of the author of the comment.</param>
		/// <param name="postId">Post id.</param>

		public async Task AddComment(PostCommentModel model, string authorId, Guid postId)
		{
			var author = await _context.Users.FirstOrDefaultAsync(u => u.Id == authorId);
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

			if (post != null && author != null)
			{
				try
				{
					var comment = new PostComment()
					{
						CommentText = model.CommentText,
						AuthorId = authorId,
						PostId = postId,
						CreatedOn = DateTime.Now,
						Author = author,
						Post = post
					};

					await _context.PostComments.AddAsync(comment);
					await _context.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					throw new ApplicationException("Adding a comment to the post failed.", ex);
				}

			}
			else
			{
				throw new ArgumentNullException("Invalid author id or blog post id.");
			}

		}
	}

}
