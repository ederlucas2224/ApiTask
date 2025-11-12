using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Task.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TasksController : ControllerBase
	{
		private readonly ITaskService _taskService;
		private readonly ILogger<TasksController> _logger;

		public TasksController(ITaskService taskService, ILogger<TasksController> logger)
		{
			_taskService = taskService;
			_logger = logger;
		}

		[HttpPost]
		public async Task<ActionResult<TaskResponse>> CreateTask([FromQuery] CreateTaskRequest request)
		{
			var task = await _taskService.CreateTaskAsync(request);
			return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAllTasks()
		{
			var tasks = await _taskService.GetAllTasksAsync();
			return Ok(tasks);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TaskResponse>> GetTask(int id)
		{
			var task = await _taskService.GetTaskByIdAsync(id);
			return Ok(task);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<TaskResponse>> UpdateTask(
			[FromRoute] int id,
			[FromQuery] UpdateTaskRequest request)
		{
			var task = await _taskService.UpdateTaskAsync(id, request);
			return Ok(task);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteTask([FromRoute] int id)
		{
			await _taskService.DeleteTaskAsync(id);
			return NoContent();
		}
	}
}
