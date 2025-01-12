using Microsoft.AspNetCore.Mvc;
using TaskBox.Shared.Models;
using TaskBox.Interfaces;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/Checkbox")]
	public class CheckboxController : Controller
	{
		private readonly ICheckboxRepository _checkboxRepository;
		public CheckboxController(ICheckboxRepository checkboxRepository)
		{
			this._checkboxRepository = checkboxRepository;
		}


		[HttpGet("GetCheckboxesFromTaskId")]
		[ProducesResponseType(200, Type=typeof(List<Checkbox>))]
		[ProducesResponseType(400)]
		public IActionResult GetCheckboxesFromTaskId(int TaskId)
		{
			try
			{
				return Ok(_checkboxRepository.GetCheckboxesFromTaskId(TaskId));
			}
			catch
			{
				return BadRequest();
			}
		}


		[HttpPost("UpdateCheckbox")]
		[ProducesResponseType(200, Type=typeof(ApiResponse))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult UpdateCheckbox(CheckboxAndUser checkboxAndUser)
		{
			try
			{
				return Ok(_checkboxRepository.UpdateCheckbox(checkboxAndUser.UserId, checkboxAndUser.Checkbox));
			}
			catch
			{
				ApiResponse response = new ApiResponse();
				response.Success = false;
				response.Message = "Something went wrong. Please try again later!";
				return BadRequest(response);
			}

		}

		[HttpPost("CreateCheckbox")]
		[ProducesResponseType(200, Type = typeof(ApiResponse))]
		[ProducesResponseType(400, Type = typeof(ApiResponse))]
		public IActionResult CreateCheckbox(CheckboxAndUser checkboxAndUser)
		{
			try
			{
				return Ok(_checkboxRepository.CreateCheckbox(checkboxAndUser.UserId, checkboxAndUser.Checkbox));
			}
			catch
			{
				ApiResponse response = new ApiResponse();
				response.Success = false;
				response.Message = "Something went wrong. Please try again later!";
				return BadRequest(response);
			}
		}
	}
}
