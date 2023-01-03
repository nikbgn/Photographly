namespace Photographly.Core.Models.Post
{
	using System;
	using System.ComponentModel.DataAnnotations;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public class CreatePostViewModel
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		[Display(Name = "Title for your new post 🔥")]
		public string Title { get; set; } = null!;

		[Required]
		[Display(Name = "Caption for your new post ⭐")]
		public string Description { get; set; } = null!;

		[FromForm]
		public IFormFile PostImage { get; set; }

		[Required]
		public DateTime CreatedOn { get; set; }

		[Required]
		public uint LikesCount { get; set; } = 0;



	}
}
