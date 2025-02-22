using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Interfaces.Repos.Reactions;

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
	}
}
