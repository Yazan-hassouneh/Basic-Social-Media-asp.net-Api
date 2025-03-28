﻿using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Interfaces.Repos.Reactions;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories;
using BasicSocialMedia.Infrastructure.Repositories.M2M;
using BasicSocialMedia.Infrastructure.Repositories.ReactionRepos;
using Microsoft.AspNetCore.Identity;

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
			PostReactions = new PostReactionsRepository(_context);
			CommentReactions = new CommentReactionsRepository(_context);
		}
		public async Task<int> Complete() => await _context.SaveChangesAsync();
		public void Dispose() => _context.Dispose();
	}
}
