using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Models.FileModels
{
	public class ProfileImageModel : IFileModel
	{
		public int Id { get; set; }
		public bool Current { get; set; } = true;
		public string UserId { get; set; } = null!;
		public virtual ApplicationUser? User { get; set; }
		public string Path { get; set; } = null!;
		public DateTime CreatedOn { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } = null!;
	}
}
