using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message) { }
	}

	public class TaskNotFoundException : NotFoundException
	{
		public TaskNotFoundException(int taskId)
			: base($"La tarea con ID {taskId} no fue encontrada") { }
	}
}
