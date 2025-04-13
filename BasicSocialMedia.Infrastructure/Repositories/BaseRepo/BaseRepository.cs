using BasicSocialMedia.Core.Interfaces.Repos.BaseRepo;
using BasicSocialMedia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BasicSocialMedia.Infrastructure.Repositories.BaseRepo
{
	internal class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context = context;
		public async Task<bool> DoesExist(int id, CancellationToken cancellationToken)
		{
			return await _context.Set<T>().AsNoTracking().AnyAsync(p => EF.Property<int>(p, "Id") == id, cancellationToken);
		}
		public async Task<T?> GetByIdWithTrackingAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}
		public virtual async Task<T?> GetByIdAsync(int id)
		{
			return await _context.Set<T>().FindAsync(id);
		}
		public async Task<T?> FindAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(matcher);
		}		
		public async Task<T?> FindWithTrackingAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(matcher);
		}
		public async Task<IEnumerable<T?>> FindAllAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().AsNoTracking().Where(matcher).ToListAsync();
		}
		public async Task<IEnumerable<T?>> FindAllWithTrackingAsync(Expression<Func<T, bool>> matcher)
		{
			return await _context.Set<T>().Where(matcher).ToListAsync();
		}
		public async Task<T> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return entity;
		}
		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
		{
			try
			{
				await _context.Set<T>().AddRangeAsync(entities);
				return entities;
			}
			catch (Exception ex)
			{
				throw new Exception("Error adding range of entities", ex);
			}
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
		public void DeleteRange(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
		}
		public async Task<int> Save()
		{
			return await _context.SaveChangesAsync();
		}
	}
}
