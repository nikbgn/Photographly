namespace Photographly.Core.Models.Post
{
	using Photographly.Infrastructure.Data.Models;

	public class ViewPostViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public byte[] PostImage { get; set; }

		public DateTime CreatedOn { get; set; }

		public uint LikesCount { get; set; }

		public List<PostComment> PostComments { get; set; } = new List<PostComment>();

	}
}
