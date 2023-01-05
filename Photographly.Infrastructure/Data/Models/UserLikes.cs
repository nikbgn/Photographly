namespace Photographly.Infrastructure.Data.Models
{
	using System.ComponentModel.DataAnnotations.Schema;
	using System.ComponentModel.DataAnnotations;

	public class UserLikes
	{
		[Required]
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		[Required]
		public Guid PostId { get; set; }

		[ForeignKey(nameof(PostId))]
		public Post Post { get; set; }
	}
}
