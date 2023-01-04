namespace Microsoft.Extensions.DependencyInjection
{
	using Photographly.Services.Contracts;
	using Photographly.Services.Services.CheckerService;
	using Photographly.Services.Services.Post;

	public static class PhotographlyServiceCollectionExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IPostService, PostService>();
			services.AddScoped<ICheckerService, CheckerService>();

			return services;
		}
	}
}
