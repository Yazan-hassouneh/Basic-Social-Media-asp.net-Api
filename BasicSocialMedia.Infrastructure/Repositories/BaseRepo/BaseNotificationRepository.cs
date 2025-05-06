using BasicSocialMedia.Core.Interfaces.ModelsInterfaces;
using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.BaseRepo
{
	internal class BaseNotificationRepository<T>(ApplicationDbContext context) : BaseRepository<T>(context), IBaseNotificationRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().Where(matcher).OrderDescending().AsNoTracking().ToListAsync();
		}
	}
}
