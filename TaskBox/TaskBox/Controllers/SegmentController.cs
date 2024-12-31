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
		public readonly AuthenticationStateProvider _authenticationStateProvider;
		public SegmentController(ISegmentRepository segmentRepository, AuthenticationStateProvider authenticationStateProvider)
		{
			this._segmentRepository = segmentRepository;
			this._authenticationStateProvider = authenticationStateProvider;
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
		public async Task<IActionResult> CreateSegment(SegmentAndUser newSegment)
		{
			ApiResponse response = new ApiResponse();
			try
			{
				return Ok(await _segmentRepository.CreateSegment(newSegment.Segment, newSegment.UserId));
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
