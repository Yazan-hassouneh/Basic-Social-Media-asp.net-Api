
using BasicSocialMedia.Core.Models.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Interfaces.ModelsInterfaces
{
	public interface IPostComment : IId, ITimestamp, IContent
	{
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; }
		public bool IsDeleted { get; set; }
		public string UserId { get; set; }
		public ApplicationUser? User { get; set; }
	}
}
