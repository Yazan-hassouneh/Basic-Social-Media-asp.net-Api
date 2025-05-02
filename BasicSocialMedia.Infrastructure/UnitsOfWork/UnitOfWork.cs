using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Interfaces.Repos.Auth;
using BasicSocialMedia.Core.Interfaces.Repos.FileModelsRepositories;
using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Interfaces.Repos.MessagingRepos;
using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories;
using BasicSocialMedia.Infrastructure.Repositories.Auth;
using BasicSocialMedia.Infrastructure.Repositories.FileRepos;
using BasicSocialMedia.Infrastructure.Repositories.M2M;
using BasicSocialMedia.Infrastructure.Repositories.MessagingRepos;
using BasicSocialMedia.Infrastructure.Repositories.ReactionRepos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace BasicSocialMedia.Infrastructure.UnitsOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public IPostRepository Posts { get; private set; }
		public ICommentRepository Comments { get; private set; }
		public IPostReactionRepository PostReactions { get; private set; }
		public ICommentReactionRepository CommentReactions { get; private set; }
		public IMessageRepository Messages { get; private set; }
		public IChatRepository Chats { get; private set; }
		public IFollowRepository Following { get; private set; }
		public IFriendshipRepository Friendship { get; private set; }
		public IBlockRepository Blocking { get; private set; }
		public IPostFileModelRepository PostFiles { get; private set; }
		public ICommentFileModelRepository CommentFiles { get; private set; }
		public IMessageFileModelRepository MessageFiles { get; private set; }
		public IProfileImageModelRepository ProfileImages { get; private set; }
		public IDeletedMessagesRepository DeletedMessages { get; private set; }
		public IChatDeletionRepository ChatDeletion { get; private set; }
		public IUserBackgroundJobs UserBackgroundJobs { get; private set; }

		public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			Chats = new ChatRepository(_context);
			Posts = new PostRepository(_context);
			Blocking = new BlockRepository(_context);
			Following = new FollowRepository(_context);
			Comments = new CommentRepository(_context);
			Messages = new MessagesRepository(_context);
			Friendship = new FriendshipRepository(_context);
			PostFiles = new PostFileModelRepository(_context);
			ChatDeletion = new ChatDeletionRepository(_context);
			PostReactions = new PostReactionsRepository(_context);
			CommentFiles = new CommentFileModelRepository(_context);
			MessageFiles = new MessageFileModelRepository(_context);
			ProfileImages = new ProfileImageModelRepository(_context);
			DeletedMessages = new DeletedMessagesRepository(_context);
			CommentReactions = new CommentReactionsRepository(_context);
			UserBackgroundJobs = new UserBackgroundJobsRepository(_context);
		}
		public async Task<int> Complete() => await _context.SaveChangesAsync();
		public Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return _context.Database.BeginTransactionAsync();
		}
		public void Dispose() => _context.Dispose();
	}
}
