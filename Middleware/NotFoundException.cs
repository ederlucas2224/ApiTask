using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
	public class NotFoundException : Exception
	{
		/// <summary>
		/// Excepcion not found
		/// </summary>
		/// <param name="message"></param>
		public NotFoundException(string message) : base(message) { }
	}

	public class TaskNotFoundException : NotFoundException
	{
		/// <summary>
		/// Excepcion no encontrada
		/// </summary>
		/// <param name="taskId"></param>
		public TaskNotFoundException(int taskId)
			: base($"La tarea con ID {taskId} no fue encontrada") { }
	}
}
