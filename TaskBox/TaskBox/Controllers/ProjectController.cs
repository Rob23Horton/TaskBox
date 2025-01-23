using Microsoft.AspNetCore.Mvc;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

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
		[ProducesResponseType(200, Type = typeof(bool))]
		[ProducesResponseType(400, Type = typeof(bool))]
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


		[HttpGet("GetProject")]
		[ProducesResponseType(200, Type = typeof(Project))]
		[ProducesResponseType(400)]
		public IActionResult GetProjectDetails(int ProjectId)
		{
			try
			{
				return Ok(_projectRepository.GetProject(ProjectId));
			}
			catch
			{
				return BadRequest();
			}

		}

		[HttpGet("GetUserProjects")]
		[ProducesResponseType(200, Type = typeof(List<Project>))]
		[ProducesResponseType(400)]
		public IActionResult GetProjectsFromUser(int UserId)
		{
			try
			{
				return Ok(_projectRepository.GetUserProjects(UserId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpGet("GetProjectUserPermissions")]
		[ProducesResponseType(200, Type=typeof(List<ProjectUserPermission>))]
		[ProducesResponseType(400)]
		public IActionResult GetUserPermissionssFromProject(int ProjectId)
		{
			try
			{
				return Ok(_projectRepository.GetPermissionsForProject(ProjectId));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("CreateUserPermission")]
		[ProducesResponseType(200, Type = typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult CreateUserPermission(PermissionAndUser PermissionAndUser)
		{
			try
			{
				return Ok(_projectRepository.CreateUserPermission(PermissionAndUser.UserId, PermissionAndUser.Permission));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("UpdateUserPermission")]
		[ProducesResponseType(200, Type = typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult UpdateUserPermission(PermissionAndUser PermissionAndUser)
		{
			try
			{
				return Ok(_projectRepository.UpdateUserPermission(PermissionAndUser.UserId, PermissionAndUser.Permission));
			}
			catch
			{
				return BadRequest();
			}
		}


		[HttpPost("CreateProject")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult CreateProject(ProjectAndUser projectAndUser)
		{
			try
			{
				return Ok(_projectRepository.CreateProject(projectAndUser.UserId, projectAndUser.Project));
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPost("UpdateProject")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400)]
		public IActionResult UpdateProject(ProjectAndUser projectAndUser)
		{
			try
			{
				return Ok(_projectRepository.UpdateProject(projectAndUser.UserId, projectAndUser.Project));
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
