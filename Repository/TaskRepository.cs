using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Linq.Expressions;

namespace Repository
{
	/// <summary>
	/// Implementación del repositorio para operaciones de acceso a datos de la entidad TaskItem
	/// Proporciona métodos específicos para gestionar tareas en la base de datos
	/// </summary>
	public class TaskRepository : ITaskRepository
	{
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Inicializa una nueva instancia del repositorio con el contexto de base de datos
		/// </summary>
		/// <param name="context">Contexto de Entity Framework para operaciones de datos</param>
		public TaskRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Obtiene todas las tareas de la base de datos ordenadas por fecha de creación descendente
		/// </summary>
		/// <returns>Colección enumerable de todas las tareas disponibles</returns>
		public async Task<IEnumerable<TaskItem>> GetAllAsync()
		{
			return await _context.Tasks
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}

		/// <summary>
		/// Obtiene una tarea específica por su identificador único
		/// </summary>
		/// <param name="id">Identificador de la tarea a buscar</param>
		/// <returns>La tarea encontrada o null si no existe</returns>
		public async Task<TaskItem?> GetByIdAsync(int id)
		{
			return await _context.Tasks
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		/// <summary>
		/// Agrega una nueva tarea a la base de datos de manera asíncrona
		/// </summary>
		/// <param name="entity">Entidad TaskItem a agregar</param>
		/// <returns>La entidad agregada con los valores generados por la base de datos</returns>
		public async Task<TaskItem> AddAsync(TaskItem entity)
		{
			var result = await _context.Tasks.AddAsync(entity);
			return result.Entity;
		}

		/// <summary>
		/// Actualiza una tarea existente en la base de datos marcándola como modificada
		/// </summary>
		/// <param name="entity">Entidad TaskItem con los datos actualizados</param>
		/// <returns>La entidad actualizada</returns>
		public async Task<TaskItem> UpdateAsync(TaskItem entity)
		{
			var result = _context.Tasks.Update(entity);
			_context.Entry(entity).State = EntityState.Modified;
			return await Task.FromResult(result.Entity);
		}

		/// <summary>
		/// Elimina una tarea de la base de datos por su identificador único
		/// </summary>
		/// <param name="id">Identificador de la tarea a eliminar</param>
		/// <returns>True si la tarea fue eliminada exitosamente, False si no se encontró la tarea</returns>
		public async Task<bool> DeleteAsync(int id)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task == null)
				return false;

			_context.Tasks.Remove(task);
			return true;
		}

		/// <summary>
		/// Verifica si existe alguna tarea que cumpla con el predicado especificado
		/// </summary>
		/// <param name="predicate">Expresión lambda para filtrar las tareas</param>
		/// <returns>True si existe al menos una tarea que cumple la condición, False en caso contrario</returns>
		public async Task<bool> ExistsAsync(Expression<Func<TaskItem, bool>> predicate)
		{
			return await _context.Tasks.AnyAsync(predicate);
		}

		/// <summary>
		/// Obtiene todas las tareas que tienen un estado específico, ordenadas por fecha de creación descendente
		/// </summary>
		/// <param name="status">Estado de las tareas a filtrar</param>
		/// <returns>Colección enumerable de tareas con el estado especificado</returns>
		public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Model.TaskStatus status)
		{
			return await _context.Tasks
				.Where(t => t.Status == status)
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}

		/// <summary>
		/// Obtiene las tareas creadas recientemente dentro de un número específico de días
		/// </summary>
		/// <param name="days">Número de días hacia atrás para considerar como "reciente"</param>
		/// <returns>Colección enumerable de tareas recientes ordenadas por fecha descendente</returns>
		public async Task<IEnumerable<TaskItem>> GetRecentTasksAsync(int days)
		{
			var dateThreshold = DateTime.UtcNow.AddDays(-days);
			return await _context.Tasks
				.Where(t => t.CreationDate >= dateThreshold)
				.OrderByDescending(t => t.CreationDate)
				.ToListAsync();
		}
	}
}