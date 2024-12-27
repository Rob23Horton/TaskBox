using Microsoft.AspNetCore.Mvc;
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
	}
}
