using BasicSocialMedia.Core.Enums;
using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.M2M
{
	internal class FriendshipRepository(ApplicationDbContext context) : BaseRepository<Friendship>(context), IFriendshipRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Friendship?> GetByIdAsync(int id)
		{
			Friendship? friendship = await _context.Friendships
				.Include(friendship => friendship.Sender)
					.ThenInclude(user => user!.ProfileImageModel)
				.Include(friendship => friendship.Receiver)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(friendship => new Friendship
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					SenderId = friendship.SenderId,
					ReceiverId = friendship.ReceiverId,
					// ... other properties of friendship ...
					Sender = friendship.Sender == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Sender.Id,
						UserName = friendship.Sender.UserName,
						ProfileImageModel = friendship.Sender.ProfileImageModel
					},
					Receiver = friendship.Receiver == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Receiver.Id,
						UserName = friendship.Receiver.UserName,
						ProfileImageModel = friendship.Receiver.ProfileImageModel
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return friendship;
		}
		public async Task<IEnumerable<Friendship?>> GetAllFriendsAsync(string userId)
		{
			return await _context.Friendships
				.Where(friendship => friendship.Status == ProjectEnums.FriendshipStatus.Accepted)
				.Where(friendship => friendship.SenderId == userId || friendship.ReceiverId == userId)
				.Include(friendship => friendship.SenderId == userId ? friendship.Sender : friendship.Receiver)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(friendship => new Friendship // Or friendshipViewModel
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					Status = friendship.Status,
					SenderId = friendship.SenderId == userId ? string.Empty : friendship.ReceiverId,
					ReceiverId = friendship.ReceiverId == userId ? string.Empty : friendship.SenderId,
					// ... other properties of friendship ...
					Sender = friendship.Sender == null || friendship.SenderId == userId ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Sender.Id,
						UserName = friendship.Sender.UserName,
						ProfileImageModel = friendship.Sender.ProfileImageModel
					},
					Receiver = friendship.Receiver == null || friendship.ReceiverId == userId ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Receiver.Id,
						UserName = friendship.Receiver.UserName,
						ProfileImageModel = friendship.Receiver.ProfileImageModel
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}
		public async Task<IEnumerable<string>> GetAllFriendsIdsAsync(string userId)
		{
			IEnumerable<string> Ids = await _context.Friendships
				.Where(friendship => friendship.Status == ProjectEnums.FriendshipStatus.Accepted)
				.Where(friendship => friendship.SenderId == userId || friendship.ReceiverId == userId)
				.Select(friendship => friendship.SenderId == userId ? friendship.ReceiverId : friendship.SenderId)
				.AsNoTracking()
				.ToListAsync();

			return Ids ?? Enumerable.Empty<string>();
		}
		public async Task<IEnumerable<Friendship?>> GetAllSentFriendRequestsAsync(string userId)
		{
			return await _context.Friendships
				.Where(friendship => friendship.SenderId == userId && friendship.Status == ProjectEnums.FriendshipStatus.Pending)
				.Include(friendship => friendship.Receiver)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(friendship => new Friendship // Or friendshipViewModel
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					Status = friendship.Status,
					SenderId = string.Empty,
					ReceiverId = friendship.ReceiverId,
					Receiver = friendship.Receiver == null || friendship.ReceiverId == userId ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Receiver.Id,
						UserName = friendship.Receiver.UserName,
						ProfileImageModel = friendship.Receiver.ProfileImageModel
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}		
		public async Task<IEnumerable<Friendship?>> GetAllPendingFriendRequestsAsync(string userId)
		{
			return await _context.Friendships
				.Where(friendship => friendship.ReceiverId == userId && friendship.Status == ProjectEnums.FriendshipStatus.Pending)
				.Include(friendship => friendship.Sender)
					.ThenInclude(user => user!.ProfileImageModel)
				.Select(friendship => new Friendship // Or friendshipViewModel
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					Status = friendship.Status,
					ReceiverId = string.Empty,
					SenderId = friendship.SenderId,
					Sender = friendship.Sender == null || friendship.SenderId == userId ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.Sender.Id,
						UserName = friendship.Sender.UserName,
						ProfileImageModel = friendship.Sender.ProfileImageModel
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}

	}
}
