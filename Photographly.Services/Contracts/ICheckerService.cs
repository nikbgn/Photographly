namespace Photographly.Services.Contracts
{
	public interface ICheckerService
	{
		/// <summary>
		/// Checks if the user is the post's author.
		/// </summary>
		/// <param name="userId">User's id</param>
		/// <param name="postId">Post's id</param>

		public Task<bool> CheckIfUserIsPostAuthor(string userId, Guid postId);
	}
}
