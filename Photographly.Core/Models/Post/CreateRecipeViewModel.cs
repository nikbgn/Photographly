namespace Photographly.Core.Models.Post
{
	using System;
	using System.ComponentModel.DataAnnotations;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public class CreateRecipeViewModel
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public string Title { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;

		[FromForm]
		public IFormFile RecipeImage { get; set; }

		[Required]
		public DateTime CreatedOn { get; set; }

		[Required]
		public uint LikesCount { get; set; } = 0;



	}
}
