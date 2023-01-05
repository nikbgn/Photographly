namespace Photographly.Core.Models.Post
{
	using Photographly.Infrastructure.Data.Models;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.ComponentModel.DataAnnotations;

	public class PostCommentModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string CommentText { get; set; } = null!;

		[Required]
		public DateTime CreatedOn { get; set; }

		[Required]
		public string AuthorId { get; set; } = null!;

		[Required]
		public Guid PostId { get; set; }

	}
}
