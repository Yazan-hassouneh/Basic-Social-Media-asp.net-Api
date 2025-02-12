using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Interfaces.ModelsInterfaces
{
	public interface IChatMessage : IId, ITimestamp
	{
		public string User1Id { get; set; }
		public string User2Id { get; set; }
	}
}
