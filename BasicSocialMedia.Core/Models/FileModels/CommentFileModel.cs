﻿using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.MainModels;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Models.FileModels
{
	public class CommentFileModel : IFileModel
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public int CommentId { get; set; }
		public virtual Comment? Comment { get; set; }
		public int PostId { get; set; }
		public virtual Post? Post { get; set; }
		public string Path { get; set; } = null!;
		public DateTime CreatedOn { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } = null!;
	}
}
