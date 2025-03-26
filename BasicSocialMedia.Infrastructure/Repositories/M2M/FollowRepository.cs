using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.M2M
{
	internal class FollowRepository(ApplicationDbContext context) : BaseRepository<Follow>(context), IFollowRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Follow?> GetByIdAsync(int id)
		{
			Follow? follow = await _context.Follows
				.Include(follow => follow.Follower)
				.Include(follow => follow.FollowingUser)
				.Select(follow => new Follow
				{
					Id = follow.Id,
					CreatedOn = follow.CreatedOn,
					FollowerId = follow.FollowerId,
					FollowingId = follow.FollowingId,
					// ... other properties of follow ...
					Follower = follow.Follower == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = follow.Follower.Id,
						UserName = follow.Follower.UserName,
						ProfileImage = follow.Follower.ProfileImage
					},
					FollowingUser = follow.FollowingUser == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = follow.FollowingUser.Id,
						UserName = follow.FollowingUser.UserName,
						ProfileImage = follow.FollowingUser.ProfileImage
					}
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return follow;
		}
		public async Task<IEnumerable<Follow?>> GetAllFollowersAsync(string userId)
		{
			return await _context.Follows
				.Where(follow => follow.FollowingId == userId)
				.Include(follow => follow.Follower)
				.Select(follow => new Follow // Or followViewModel
				{
					Id = follow.Id,
					CreatedOn = follow.CreatedOn,
					FollowerId = follow.FollowerId,
					// ... other properties of follow ...
					Follower = follow.Follower == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = follow.Follower.Id,
						UserName = follow.Follower.UserName,
						ProfileImage = follow.Follower.ProfileImage
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}
		public async Task<IEnumerable<Follow?>> GetAllFollowingsAsync(string userId)
		{
			return await _context.Follows
				.Where(follow => follow.FollowerId == userId)
				.Include(follow => follow.FollowingUser)
				.Select(follow => new Follow // Or followViewModel
				{
					Id = follow.Id,
					CreatedOn = follow.CreatedOn,
					FollowingId = follow.FollowingId,
					// ... other properties of follow ...
					FollowingUser = follow.FollowingUser == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = follow.FollowingUser.Id,
						UserName = follow.FollowingUser.UserName,
						ProfileImage = follow.FollowingUser.ProfileImage
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
