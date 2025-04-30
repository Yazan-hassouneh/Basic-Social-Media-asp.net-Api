using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Interfaces.Repos.FileModelsRepositories;
using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos;
using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace BasicSocialMedia.Core.Interfaces.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		public IPostRepository Posts { get; }
		public ICommentRepository Comments { get; }
		public IPostReactionRepository PostReactions { get; }
		public ICommentReactionRepository CommentReactions { get; }
		public IMessageRepository Messages { get; }
		public IChatRepository Chats { get; }
		public IFollowRepository Following { get; }
		public IFriendshipRepository Friendship { get; }
		public IBlockRepository Blocking { get; }
		public IPostFileModelRepository PostFiles { get; }
		public IChatDeletionRepository ChatDeletion { get; }
		public ICommentFileModelRepository CommentFiles { get; }
		public IMessageFileModelRepository MessageFiles { get; }
		public IProfileImageModelRepository ProfileImages { get; }
		public IDeletedMessagesRepository DeletedMessages { get; }
		Task<IDbContextTransaction> BeginTransactionAsync();

	}
}
