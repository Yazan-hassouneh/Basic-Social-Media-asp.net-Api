using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;

namespace BasicSocialMedia.Core.Models.AuthModels
{
	public class UserBackgroundJob : IId
	{
		public int Id { get; set; }
		public string UserId { get; set; } = null!;
		public string JobId { get; set; } = null!;
		public string JobType { get; set; } = null!;
		public DateTime ScheduledAt { get; set; }
	}
}
