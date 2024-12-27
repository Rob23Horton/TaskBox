using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/Project")]
	public class ProjectController : Controller
	{
		private readonly IProjectRepository _projectRepository;
		public ProjectController(IProjectRepository projectRepository)
		{
			this._projectRepository = projectRepository;
		}

		[HttpGet("CheckPermission")]
		[ProducesResponseType(200, Type=typeof(bool))]
		[ProducesResponseType(400, Type=typeof(bool))]
		public IActionResult CheckUserPermission(int UserId, int ProjectId)
		{
			try
			{
				return Ok(_projectRepository.CheckProjectPermission(UserId, ProjectId));
			}
			catch
			{
				return BadRequest(false);
			}
		}
	}
}
