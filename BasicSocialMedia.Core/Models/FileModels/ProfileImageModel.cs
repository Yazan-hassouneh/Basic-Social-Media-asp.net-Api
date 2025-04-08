using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Models.FileModels
{
	public class ProfileImageModel : IFileModel
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public string Path { get; set; } = null!;
		public DateTime CreatedOn { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } = null!;
	}
}
