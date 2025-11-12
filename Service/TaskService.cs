using Data;
using Microsoft.EntityFrameworkCore;
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

		/// <summary>
		/// Crea una nueva tarea en el sistema después de validar que no exista una con el mismo título
		/// </summary>
		/// <param name="request">Datos para la creación de la tarea</param>
		/// <returns>Respuesta con los datos de la tarea creada</returns>
		/// <exception cref="BusinessException">Se lanza cuando ya existe una tarea con el mismo título</exception>
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

		/// <summary>
		/// Obtiene todas las tareas del sistema ordenadas por fecha de creación descendente
		/// </summary>
		/// <returns>Lista de todas las tareas disponibles</returns>
		public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
		{
			var tasks = await _unitOfWork.Tasks.GetAllAsync();
			return tasks.Select(MapToTaskResponse);
		}

		/// <summary>
		/// Obtiene una tarea específica por su identificador único
		/// </summary>
		/// <param name="id">Identificador de la tarea a buscar</param>
		/// <returns>Datos de la tarea encontrada</returns>
		/// <exception cref="TaskNotFoundException">Se lanza cuando no se encuentra la tarea con el ID especificado</exception>
		public async Task<TaskResponse?> GetTaskByIdAsync(int id)
		{
			var task = await _unitOfWork.Tasks.GetByIdAsync(id);

			if (task == null)
				throw new TaskNotFoundException(id);

			return MapToTaskResponse(task);
		}

		/// <summary>
		/// Actualiza una tarea existente permitiendo actualizar solo los campos proporcionados
		/// </summary>
		/// <param name="id">Identificador de la tarea a actualizar</param>
		/// <param name="request">Datos parciales para la actualización</param>
		/// <returns>Respuesta con los datos actualizados de la tarea</returns>
		/// <exception cref="NotFoundException">Se lanza cuando no se encuentra la tarea con el ID especificado</exception>
		public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request)
		{
			await _unitOfWork.BeginTransactionAsync();
			var existingTask = await _unitOfWork.Tasks.GetByIdAsync(id);
			if (existingTask == null)
				throw new NotFoundException($"Task with ID {id} not found");

			// Actualizar solo los campos que no son nulos
			if (request.Title != null)
				existingTask.Title = request.Title;

			if (request.Description != null)
				existingTask.Description = request.Description;

			if (request.Status.HasValue)
				existingTask.Status = request.Status.Value;

			await _unitOfWork.SaveChangesAsync();
			await _unitOfWork.CommitTransactionAsync();
			return MapToTaskResponse(existingTask);
		}

		/// <summary>
		/// Elimina una tarea del sistema por su identificador único
		/// </summary>
		/// <param name="id">Identificador de la tarea a eliminar</param>
		/// <returns>True si la tarea fue eliminada exitosamente, False si no se encontró la tarea</returns>
		/// <exception cref="TaskNotFoundException">Se lanza cuando no se encuentra la tarea con el ID especificado</exception>
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

		/// <summary>
		/// Mapea una entidad TaskItem a un objeto TaskResponse para la respuesta API
		/// </summary>
		/// <param name="task">Entidad de tarea a mapear</param>
		/// <returns>Objeto de respuesta formateado para la API</returns>
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