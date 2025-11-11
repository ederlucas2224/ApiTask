using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Linq.Expressions;

namespace Repository
{
	public class TaskRepository : ITaskRepository
	{
		private readonly ApplicationDbContext _context;

		public TaskRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<TaskItem>> GetAllAsync()
		{
			return await _context.Tasks
				.AsNoTracking()
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}

		public async Task<TaskItem?> GetByIdAsync(int id)
		{
			return await _context.Tasks
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task<TaskItem> AddAsync(TaskItem entity)
		{
			var result = await _context.Tasks.AddAsync(entity);
			return result.Entity;
		}

		public async Task<TaskItem> UpdateAsync(TaskItem entity)
		{
			var result = _context.Tasks.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
			return await Task.FromResult(result.Entity);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task == null)
				return false;

			_context.Tasks.Remove(task);
			return true;
		}

		public async Task<bool> ExistsAsync(Expression<Func<TaskItem, bool>> predicate)
		{
			return await _context.Tasks.AnyAsync(predicate);
		}

		public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Model.TaskStatus status)
		{
			return await _context.Tasks
				.AsNoTracking()
				.Where(t => t.Status == status)
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}

		public async Task<IEnumerable<TaskItem>> GetRecentTasksAsync(int days)
		{
			var dateThreshold = DateTime.UtcNow.AddDays(-days);
			return await _context.Tasks
				.AsNoTracking()
				.Where(t => t.CreationDate >= dateThreshold)
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}
	}
}
