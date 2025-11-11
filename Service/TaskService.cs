using Data;
using Microsoft.Extensions.Logging;
using Middleware;
using Model;
using Repository;
using TaskStatus = Model.TaskStatus;

namespace Service
{
	public class TaskService : ITaskService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<TaskService> _logger;

		public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();

				// Validación de negocio adicional
				if (await _unitOfWork.Tasks.ExistsAsync(t => t.Title == request.Title.Trim()))
					throw new BusinessException("Ya existe una tarea con el mismo título");

				var task = new TaskItem
				{
					Title = request.Title.Trim(),
					Description = request.Description?.Trim(),
					Status = TaskStatus.Pending,
					CreationDate = DateTime.UtcNow
				};

				var createdTask = await _unitOfWork.Tasks.AddAsync(task);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitTransactionAsync();

				_logger.LogInformation("Tarea creada exitosamente con ID: {TaskId}", createdTask.Id);

				return MapToTaskResponse(createdTask);
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
		{
			var tasks = await _unitOfWork.Tasks.GetAllAsync();
			return tasks.Select(MapToTaskResponse);
		}

		public async Task<TaskResponse?> GetTaskByIdAsync(int id)
		{
			var task = await _unitOfWork.Tasks.GetByIdAsync(id);

			if (task == null)
				throw new TaskNotFoundException(id);

			return MapToTaskResponse(task);
		}

		public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();

				var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);
				if (existingTask == null)
					throw new TaskNotFoundException(id);

				// Validación de negocio adicional
				if (await _unitOfWork.Tasks.ExistsAsync(t => t.Title == request.Title.Trim() && t.Id != id))
					throw new BusinessException("Ya existe otra tarea con el mismo título");

				existingTask.Title = request.Title.Trim();
				existingTask.Description = request.Description?.Trim();
				existingTask.Status = request.Status;

				var updatedTask = await _unitOfWork.Tasks.UpdateAsync(existingTask);
				await _unitOfWork.SaveChangesAsync();
				await _unitOfWork.CommitTransactionAsync();

				_logger.LogInformation("Tarea actualizada exitosamente con ID: {TaskId}", id);

				return MapToTaskResponse(updatedTask);
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<bool> DeleteTaskAsync(int id)
		{
			try
			{
				await _unitOfWork.BeginTransactionAsync();

				var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);
				if (existingTask == null)
					throw new TaskNotFoundException(id);

				var deleted = await _unitOfWork.Tasks.DeleteAsync(id);
				if (deleted)
				{
					await _unitOfWork.SaveChangesAsync();
					await _unitOfWork.CommitTransactionAsync();
					_logger.LogInformation("Tarea eliminada exitosamente con ID: {TaskId}", id);
				}

				return deleted;
			}
			catch (Exception)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		private static TaskResponse MapToTaskResponse(TaskItem task)
		{
			return new TaskResponse
			{
				Id = task.Id,
				Title = task.Title,
				Description = task.Description,
				Status = task.Status.ToString(),
				CreationDate = task.CreationDate
			};
		}
	}
}
