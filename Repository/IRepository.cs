using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T?> GetByIdAsync(int id);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task<bool> DeleteAsync(int id);
		Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
	}
}
