using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/LogTime")]
	public class LogTimeController : Controller
	{
		private readonly ILogTimeRepository _logTimeRepositoy;
		public LogTimeController(ILogTimeRepository logTimeRepository)
		{
			this._logTimeRepositoy = logTimeRepository;
		}

		[HttpGet("GetTimeLogs")]
		[ProducesResponseType(200, Type=typeof(List<TimeLog>))]
		[ProducesResponseType(400)]
		public IActionResult GetAllTimeLogsFromUser(int UserId)
		{
			try
			{
				return Ok(_logTimeRepositoy.GetAllTimeLogsForUser(UserId));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
