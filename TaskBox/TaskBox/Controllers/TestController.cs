using Microsoft.AspNetCore.Mvc;
using TaskBox.Shared.Models;
using DatabaseConnection.Services;
using DatabaseConnection.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api")]
	public class ApiController : Controller
	{
		private readonly IDatabaseConnection _databaseConnection;

		public ApiController(IDatabaseConnection _databaseConnection)
		{
			this._databaseConnection = _databaseConnection;
		}

		[HttpGet("TestDatabase")]
		public IActionResult Get()
		{
			SelectRequest request = new SelectRequest("tblUser");
			request.AddData("tblUser", "UserId", "Id");
			request.AddData("UserName");
			request.AddData("Password");

			request.AddWhere("UserId", 1);

			List<User> result = _databaseConnection.Select<User>(request);

			return Ok(result);
		}

		[HttpGet("TestDatabase2")]
		public IActionResult GetTwo()
		{
			SelectRequest request = new SelectRequest("tblUser");
			request.AddData("tblProjectUser", "ProjectUserId", "Id");
			request.AddData("tblUser", "UserName");
			request.AddData("tblUser", "UserId", "UserId");
			request.AddData("tblProjectUser", "ProjectCode", "ProjectId");
			request.AddData("tblProjectUser", "Permission");

			request.AddJoin("tblUser", "UserId", "tblProjectUser", "UserCode");

			request.AddWhere("tblUser", "UserId", 1);

			List<UserProject> result = _databaseConnection.Select<UserProject>(request);

			return Ok(result);
		}

		[HttpGet("TestDatabase3")]
		public IActionResult GetThree()
		{
			InsertRequest noteInsert = new InsertRequest("tblNote");
			Note newNote = new Note("Hello, This is a test note for a test Project");

			_databaseConnection.Insert<Note>(noteInsert, newNote);

			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", newNote.Description);

			int NoteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;


			InsertRequest request = new InsertRequest("tblProject");

			request.AddData("Name", "Test Project");
			request.AddData("Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			request.AddData("Due", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			request.AddData("NoteCode", NoteCode);

			_databaseConnection.Insert(request);

			return Ok();
		}
	}
}
