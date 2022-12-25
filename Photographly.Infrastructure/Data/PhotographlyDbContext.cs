namespace Photographly.Infrastructure.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;

	public class PhotographlyDbContext : IdentityDbContext
	{
		public PhotographlyDbContext(DbContextOptions<PhotographlyDbContext> options)
			: base(options)
		{
		}
	}
}