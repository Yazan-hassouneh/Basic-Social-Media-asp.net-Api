using System.Linq.Expressions;

namespace BasicSocialMedia.Core.Interfaces.Repos.BaseRepo
{
	public interface IBaseRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(int id);
		Task<IEnumerable<T?>> GetAllAsync();
		Task<T?> FindAsync(Expression<Func<T, bool>> matcher);
		Task<IEnumerable<T?>> FindAllAsync(Expression<Func<T, bool>> matcher);
		Task<T> AddAsync(T entity);
		T Update(T entity);
		void Delete(T entity);
		Task<int> Save();
	}
}
