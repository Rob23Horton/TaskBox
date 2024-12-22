using Microsoft.AspNetCore.Mvc;
using TaskBox.Shared.Models;
using DatabaseConnection.Services;
using DatabaseConnection.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api")]
	public class ApiController : Controller
	{
		private static readonly string[] Summaries = new[]
		{"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};

		private readonly IDatabaseConnection _databaseConnection;

		public ApiController(IDatabaseConnection _databaseConnection)
		{
			this._databaseConnection = _databaseConnection;
		}

		[HttpGet("TestDatabase")]
		public string Get()
		{
			SelectRequest request = new SelectRequest();

			return "Finished";
		}
	}
}
