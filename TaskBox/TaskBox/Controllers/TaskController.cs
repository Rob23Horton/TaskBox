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

		[HttpGet("GetTask")]
		[ProducesResponseType(200, Type = typeof(TaskBoxTask))]
		[ProducesResponseType(400)]
		public IActionResult GetTask(int TaskId)
		{
			try
			{
				return Ok(_taskRepository.GetTask(TaskId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("CheckPermission")]
		[ProducesResponseType(200, Type=typeof(bool))]
		[ProducesResponseType(400, Type=typeof(bool))]
		public IActionResult CheckPermission(int UserId, int TaskId)
		{
			try
			{
				return Ok(_taskRepository.CheckPermission(UserId, TaskId).Permission.ToUpper() != "N"? true : false);
			}
			catch
			{
				return BadRequest(false);
			}
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
		public IActionResult CreateTask(TaskAndUser newSegment)
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
