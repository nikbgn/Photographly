namespace Photographly.Infrastructure.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	using Photographly.Infrastructure.Data.Models;

	public class PhotographlyDbContext : IdentityDbContext<User>
	{
		public PhotographlyDbContext(DbContextOptions<PhotographlyDbContext> options)
			: base(options)
		{
		}
		public DbSet<Post> Posts { get; set; }
		public DbSet<PostComment> PostComments { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.Entity<UserPost>()
				.HasKey(x => new { x.UserId, x.PostId });

			builder.Entity<UserLikes>()
				.HasKey(x => new { x.UserId, x.PostId });

			base.OnModelCreating(builder);
        }
    }
}