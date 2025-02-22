using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.BaseRepo
{
	internal class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context = context;
		public virtual async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}
		public virtual async Task<IEnumerable<T?>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}
		public async Task<T?> FindAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(matcher);
		}
		public async Task<T> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return entity;
		}
		public T Update(T entity)
		{
			_context.Set<T>().Update(entity);
			return entity;
		}
		public void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}
		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
