using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/Segment")]
	public class SegmentController : Controller
	{
		private readonly ISegmentRepository _segmentRepository;
		public SegmentController(ISegmentRepository segmentRepository)
		{
			this._segmentRepository = segmentRepository;
		}

		[HttpGet("GetSegment")]
		[ProducesResponseType(200, Type=typeof(Segment))]
		[ProducesResponseType(400)]
		public IActionResult GetSegmentFromId(int SegmentId)
		{
			try
			{
				return Ok(_segmentRepository.GetSegmentFromId(SegmentId));
			}
			catch
			{
				return BadRequest();
			}
		}


		[HttpGet("SegmentsFromProjectId")]
		[ProducesResponseType(200, Type=typeof(IEnumerable<Segment>))]
		[ProducesResponseType(400)]
		public IActionResult GetSegments(int ProjectId)
		{
			try
			{
				return Ok(_segmentRepository.GetSegmentsFromProjectId(ProjectId));
			}
			catch
			{
				return BadRequest();
			}
		}


		[HttpPost("CreateSegment")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400, Type=typeof(ApiResponse))]
		public IActionResult CreateSegment(SegmentAndUser newSegment)
		{
			ApiResponse response = new ApiResponse();
			try
			{
				return Ok(_segmentRepository.CreateSegment(newSegment.Segment, newSegment.UserId));
			}
			catch
			{
				response.Success = false;
				response.Message = "Something went wrong. Please try again later!";
				return BadRequest(response);
			}
		}

		[HttpGet("CheckPermission")]
		[ProducesResponseType(200, Type=typeof(bool))]
		[ProducesResponseType(400, Type = typeof(bool))]
		public IActionResult CheckPermission(int UserId, int SegmentId)
		{
			try
			{
				return Ok(_segmentRepository.UserSegmentPermission(SegmentId, UserId).Permission.ToUpper() == "N"? false : true);
			}
			catch
			{
				return BadRequest(false);
			}
		}
	}
}
