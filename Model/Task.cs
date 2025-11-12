using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	/// <summary>
	/// Modelo de Tareas
	/// </summary>
	public class TaskItem
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string? Description { get; set; }
		public TaskStatus Status { get; set; }
		public DateTime CreationDate { get; set; }
	}
	/// <summary>
	/// Estatus de tareas
	/// </summary>
	public enum TaskStatus
	{
		Pending,
		InProgress,
		Completed
	}
}
