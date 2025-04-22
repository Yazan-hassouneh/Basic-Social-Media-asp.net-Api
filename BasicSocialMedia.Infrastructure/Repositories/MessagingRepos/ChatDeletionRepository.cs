using BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos;
using BasicSocialMedia.Core.Models.Messaging;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;

namespace BasicSocialMedia.Infrastructure.Repositories.MessagingRepos
{
	internal class ChatDeletionRepository(ApplicationDbContext context) : BaseRepository<ChatDeletion>(context), IChatDeletionRepository
	{
	}
}
