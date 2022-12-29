namespace Photographly.Core.Models.Post
{
	public class PostServiceModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public byte[] PostImage { get; set; }

		public DateTime CreatedOn { get; set; }

		public uint LikesCount { get; set; }


	}
}
