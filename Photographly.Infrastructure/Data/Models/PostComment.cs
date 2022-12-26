namespace Photographly.Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PostComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CommentText { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = null!;

        [Required]
        public Guid PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
    }
}
