namespace Photographly.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public byte[] PostImage { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public uint LikesCount { get; set; } = 0;

        public List<PostComment> PostComments { get; set; } = new List<PostComment>();

        public List<UserPost> UsersPosts { get; set; } = new List<UserPost>();

		public List<UserLikes> UsersLikes { get; set; } = new List<UserLikes>();
	}
}
