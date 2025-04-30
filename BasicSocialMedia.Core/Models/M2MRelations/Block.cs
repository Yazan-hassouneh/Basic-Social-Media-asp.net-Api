using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Models.AuthModels;

namespace BasicSocialMedia.Core.Models.M2MRelations
{
	public class Block : IId, ITimestamp
	{
		public int Id { get; set; }
		public DateTime CreatedOn { get; set; }
		public string? BlockerId { get; set; } 
		public ApplicationUser? Blocker { get; set; }
		public string? BlockedId { get; set; }
		public ApplicationUser? Blocked { get; set; }
	}
}
