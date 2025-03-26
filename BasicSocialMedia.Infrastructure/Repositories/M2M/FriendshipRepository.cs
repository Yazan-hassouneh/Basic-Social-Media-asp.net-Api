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
				.Include(friendship => friendship.UserId1)
				.Include(friendship => friendship.UserId2)
				.Select(friendship => new Friendship
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					// ... other properties of friendship ...
					User1 = friendship.User1 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.User1.Id,
						UserName = friendship.User1.UserName,
						ProfileImage = friendship.User1.ProfileImage
					},
					User2 = friendship.User2 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.User2.Id,
						UserName = friendship.User2.UserName,
						ProfileImage = friendship.User2.ProfileImage
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return friendship;
		}
		public async Task<IEnumerable<Friendship?>> GetAllAsync(string userId)
		{
			return await _context.Friendships
				.Where(friendship => friendship.UserId1 == userId || friendship.UserId2 == userId)
				.Include(friendship => friendship.UserId1 == userId ? friendship.User2 : friendship.User1)
				.Select(friendship => new Friendship // Or friendshipViewModel
				{
					Id = friendship.Id,
					CreatedOn = friendship.CreatedOn,
					Status = friendship.Status,
					// ... other properties of friendship ...
					User1 = friendship.User1 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.User1.Id,
						UserName = friendship.User1.UserName,
						ProfileImage = friendship.User1.ProfileImage
					},
					User2 = friendship.User2 == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = friendship.User2.Id,
						UserName = friendship.User2.UserName,
						ProfileImage = friendship.User2.ProfileImage
					},					
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
