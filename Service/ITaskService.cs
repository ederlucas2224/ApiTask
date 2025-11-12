using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public interface ITaskService
	{
		Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
		Task<IEnumerable<TaskResponse>> GetAllTasksAsync();
		Task<TaskResponse?> GetTaskByIdAsync(int id);
		Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request);
		Task<bool> DeleteTaskAsync(int id);
	}
}
