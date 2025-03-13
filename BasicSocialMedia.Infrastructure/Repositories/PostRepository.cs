using BasicSocialMedia.Core.Interfaces.Repos;
using BasicSocialMedia.Core.Models.AuthModels;
using BasicSocialMedia.Core.Models.MainModels;
using BasicSocialMedia.Infrastructure.Data;
using BasicSocialMedia.Infrastructure.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace BasicSocialMedia.Infrastructure.Repositories
{
	internal class PostRepository(ApplicationDbContext context) : BaseRepository<Post>(context), IPostRepository
	{
		private readonly ApplicationDbContext _context = context;
		public override async Task<Post?> GetByIdAsync(int id)
		{
			Post? post = await _context.Posts
				.Include(post => post.User)
				.Select(post => new Post // Or ChatViewModel
				{
					Id = post.Id,
					CreatedOn = post.CreatedOn,
					Audience = post.Audience,
					Content = post.Content,
					IsDeleted = post.IsDeleted,
					User = post.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = post.User.Id,
						UserName = post.User.UserName,
						ProfileImage = post.User.ProfileImage
					},
				})
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);

			return post;
		}
		public async Task<IEnumerable<Post?>> GetAllAsync(string userId)
		{
			return await _context.Posts
				.Where(post => post.UserId == userId)
				.Include(post => post.User)
				.Select(post => new Post
				{
					Id = post.Id,
					CreatedOn = post.CreatedOn,
					Audience = post.Audience,
					Content = post.Content,
					IsDeleted = post.IsDeleted,
					User = post.User == null ? null : new ApplicationUser // Or anonymous type, handle potential nulls
					{
						Id = post.User.Id,
						UserName = post.User.UserName,
						ProfileImage = post.User.ProfileImage
					},
				})
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
