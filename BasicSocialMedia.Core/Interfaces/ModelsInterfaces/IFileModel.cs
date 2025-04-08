using System.ComponentModel.DataAnnotations;

namespace BasicSocialMedia.Core.Interfaces.ModelsInterfaces
{
	public interface IFileModel : IId, ITimestamp
	{
		public string UserId { get; set; }
		public string Path { get; set; }
		[Timestamp]  // Concurrency token
		public byte[] RowVersion { get; set; } 
	}
}
