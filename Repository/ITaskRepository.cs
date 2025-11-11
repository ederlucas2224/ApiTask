using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface ITaskRepository : IRepository<TaskItem>
	{
		Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Model.TaskStatus status);
		Task<IEnumerable<TaskItem>> GetRecentTasksAsync(int days);
	}
}
