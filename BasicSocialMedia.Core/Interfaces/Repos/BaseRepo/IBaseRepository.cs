using System.Linq.Expressions;

namespace BasicSocialMedia.Core.Interfaces.Repos.BaseRepo
{
	public interface IBaseRepository<T> where T : class
	{
		Task<bool> DoesExist(int Id, CancellationToken cancellationToken);
		Task<T?> GetByIdAsync(int id);
		Task<T?> GetByIdWithTrackingAsync(int id);
		Task<T?> FindAsync(Expression<Func<T, bool>> matcher);
		Task<T?> FindWithTrackingAsync(Expression<Func<T, bool>> matcher);
		Task<IEnumerable<T?>> FindAllAsync(Expression<Func<T, bool>> matcher);
		Task<T> AddAsync(T entity);
		T Update(T entity);
		void Delete(T entity);
		Task<int> Save();
	}
}
