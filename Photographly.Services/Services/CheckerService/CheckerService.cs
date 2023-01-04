namespace Photographly.Services.Services.CheckerService
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	using Photographly.Infrastructure.Data;
	using Photographly.Services.Contracts;

	public class CheckerService : ICheckerService
	{

		private readonly PhotographlyDbContext _context;

		public CheckerService(PhotographlyDbContext context)
		{
			_context = context;
		}


		/// <summary>
		/// Checks if the user is the post's author.
		/// </summary>
		/// <param name="userId">User's id</param>
		/// <param name="postId">Post's id</param>

		public async Task<bool> CheckIfUserIsPostAuthor(string userId, Guid postId)
		{
			try
			{
				var user = await _context
					.Users
					.Include(u => u.UsersPosts)
					.FirstOrDefaultAsync(u => u.Id == userId);


				if (user == null) throw new ArgumentException("Invalid user ID!");

				bool check = user.UsersPosts.Any(p => p.PostId == postId);
				return check;

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
