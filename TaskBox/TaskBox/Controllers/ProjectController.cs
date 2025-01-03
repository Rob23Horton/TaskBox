﻿using Microsoft.AspNetCore.Mvc;
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


		[HttpGet("GetProject")]
		[ProducesResponseType(200, Type=typeof(Project))]
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
	}
}
