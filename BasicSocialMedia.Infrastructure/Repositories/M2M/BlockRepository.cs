using BasicSocialMedia.Core.Interfaces.Repos.M2M;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.M2MRelations;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories.M2M
{
	internal class BlockRepository(ApplicationDbContext context) : BaseRepository<Block>(context), IBlockRepository
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<bool> IsBlockingAsync(string userId1, string userId2)
		{
			return await _context.Blocking
				.Where(blocking => (blocking.BlockerId == userId1 && blocking.BlockedId == userId2)
					|| (blocking.BlockedId == userId1 && blocking.BlockedId == userId2)
				)
				.AsNoTracking()
				.AnyAsync();
		}
		public async Task<IEnumerable<Block?>> GetBlockListAsync(string userId)
		{
			return await _context.Blocking
				.Where(blocking => blocking.BlockerId == userId)
				.Include(blocking => blocking.Blocked)
				.Select(blocking => new Block 
				{
					Id = blocking.Id,
					CreatedOn = blocking.CreatedOn,
					BlockerId = string.Empty,
					BlockedId = blocking.BlockedId,
					Blocked = blocking.Blocked == null || blocking.BlockedId == userId ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = blocking.Blocked.Id,
						UserName = blocking.Blocked.UserName,
						ProfileImage = blocking.Blocked.ProfileImage
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}   
	}
}
