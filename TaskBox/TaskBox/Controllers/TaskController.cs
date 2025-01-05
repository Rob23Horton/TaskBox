using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;
using TaskBox.Repositories;
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

		[HttpPost("CreateTask")]
		[ProducesResponseType(200, Type = typeof(ApiResponse))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult CreateSegment(TaskAndUser newSegment)
		{
			ApiResponse response = new ApiResponse();
			try
			{
				return Ok(_taskRepository.CreateTask(newSegment.Task, newSegment.UserId));
			}
			catch
			{

				response.Success = false;
				response.Message = "Something went wrong. Please try again later!";
				return BadRequest(response);
			}
		}
	}
}
