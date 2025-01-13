using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/Bug")]
	public class BugController : Controller
	{
		private readonly IBugRepository _bugRepository;
		public BugController(IBugRepository bugRepository)
		{
			this._bugRepository = bugRepository;
		}

		[HttpGet("GetBugs")]
		[ProducesResponseType(200, Type=typeof(List<Bug>))]
		[ProducesResponseType(400)]
		public IActionResult GetBugsFromSegment(int SegmentId)
		{
			try
			{
				return Ok(_bugRepository.GetBugsForSegment(SegmentId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("CreateBug")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult CreateBug(BugAndUser BugAndUser)
		{
			try
			{
				return Ok(_bugRepository.CreateBug(BugAndUser.Bug, BugAndUser.UserId));
			}
			catch
			{
				ApiResponse response = new ApiResponse();
				response.Success = false;
				response.Message = "An error occured. Please try again later!";
				return BadRequest(response);
			}
		}

		[HttpPost("UpdateBug")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult UpdateBug(BugAndUser BugAndUser)
		{
			try
			{
				return Ok(_bugRepository.UpdateBug(BugAndUser.Bug, BugAndUser.UserId));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
