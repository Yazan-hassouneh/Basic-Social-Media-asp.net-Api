using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Interfaces.ModelsInterfaces
{
	public interface IReactions : IId, ITimestamp
	{
		public string UserId { get; set; }
		public ApplicationUser? User { get; set; }

	}
}
