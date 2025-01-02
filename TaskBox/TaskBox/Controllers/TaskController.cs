using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/Task")]
	public class TaskController : Controller
	{
		private readonly ITaskRepository _taskRepository;
		public TaskController(ITaskRepository taskRepository)
		{
			this._taskRepository = taskRepository;
		}

		[HttpGet("TasksFromSegmentId")]
		[ProducesResponseType(200, Type=typeof(List<TaskBoxTask>))]
		[ProducesResponseType(400)]
		public IActionResult GetTasksFromSegmentId(int SegmentId)
		{
			try
			{
				return Ok(_taskRepository.GetTasksFromSegmentId(SegmentId));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
