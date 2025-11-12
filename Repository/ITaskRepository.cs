using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface ITaskRepository
	{
		Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Model.TaskStatus status);
		Task<IEnumerable<TaskItem>> GetRecentTasksAsync(int days);
		Task<IEnumerable<TaskItem>> GetAllAsync();
		Task<TaskItem?> GetByIdAsync(int id);
		Task<TaskItem> AddAsync(TaskItem entity);
		Task<TaskItem> UpdateAsync(TaskItem entity);
		Task<bool> DeleteAsync(int id);
		Task<bool> ExistsAsync(Expression<Func<TaskItem, bool>> predicate);
	}
}
