namespace Photographly.Infrastructure.Data.Models
{
	using Microsoft.AspNetCore.Identity;

	public class User : IdentityUser
	{
		public List<UserPost> UsersPosts { get; set; } = new List<UserPost>();
	}
}
