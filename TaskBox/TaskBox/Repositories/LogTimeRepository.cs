using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class LogTimeRepository : ILogTimeRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public readonly ITaskRepository _taskRepository;
		public LogTimeRepository(IDatabaseConnection databaseConnection, ITaskRepository taskRepository)
		{
			this._databaseConnection = databaseConnection;
			this._taskRepository = taskRepository;
		}

		public List<TimeLog> GetAllTimeLogsForUser(int UserId)
		{
			SelectRequest logsRequest = new SelectRequest("tblTimeLog");

			logsRequest.AddData("tblTimeLog", "TimeLogId", "Id");
			logsRequest.AddData("tblTimeLog", "Name");
			logsRequest.AddData("tblTimeLog", "TaskCode");
			logsRequest.AddData("tblTask", "Name", "TaskName");
			logsRequest.AddData("tblTimeLog", "UserCode");
			logsRequest.AddData("tblTimeLog", "Start");
			logsRequest.AddData("tblTimeLog", "End");
			logsRequest.AddData("tblNote", "Description");

			logsRequest.AddJoin("tblTimeLog", "NoteCode", "tblNote", "NoteId");
			logsRequest.AddJoin("tblTimeLog", "TaskCode", "tblTask", "TaskId");

			logsRequest.AddWhere("tblTimeLog", "UserCode", UserId);

			List<TimeLog> timeLogs = _databaseConnection.Select<TimeLog>(logsRequest);

			return timeLogs;

		}

		public ApiResponse CreateTimeLog(TimeLog TimeLog)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A, M, S or T)
			ProjectUserPermission userPermission = _taskRepository.CheckPermission(TimeLog.UserCode, TimeLog.TaskCode);

			Console.WriteLine($"User {TimeLog.UserCode} is adding Time Log {TimeLog.Name} to task {TimeLog.TaskName} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S" && userPermission.Permission.ToUpper() != "T")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Inserts into the database
			Note note = new Note(TimeLog.Description);
			InsertRequest noteInsert = new InsertRequest("tblNote");

			_databaseConnection.Insert<Note>(noteInsert, note);

			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", note.Description);

			int noteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;


			InsertRequest timeLogInsert = new InsertRequest("tblTimeLog");
			timeLogInsert.AddData("Name", TimeLog.Name);
			timeLogInsert.AddData("TaskCode", TimeLog.TaskCode);
			timeLogInsert.AddData("UserCode", TimeLog.UserCode);
			timeLogInsert.AddData("Start", TimeLog.Start);
			timeLogInsert.AddData("End", TimeLog.End);
			timeLogInsert.AddData("NoteCode", noteCode);

			_databaseConnection.Insert(timeLogInsert);

			response.Success = true;
			return response;
		}

		public ApiResponse UpdateTimeLog(TimeLog TimeLog)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A, M, S or T)
			ProjectUserPermission userPermission = _taskRepository.CheckPermission(TimeLog.UserCode, TimeLog.TaskCode);

			Console.WriteLine($"User {TimeLog.UserCode} is adding Time Log {TimeLog.Name} to task {TimeLog.TaskName} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S" && userPermission.Permission.ToUpper() != "T")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Gets note id
			SelectRequest previouseTimeLogRequest = new SelectRequest("tblTimeLog");
			previouseTimeLogRequest.AddData("tblTimeLog", "NoteCode", "Id");
			previouseTimeLogRequest.AddWhere("TimeLogId", TimeLog.Id);
			List<Note> pastNote = _databaseConnection.Select<Note>(previouseTimeLogRequest);

			//Creates and updates note
			Note newNote = new Note(pastNote[0].Id, TimeLog.Description);
			UpdateRequest noteUpdateRequest = new UpdateRequest("tblNote");
			noteUpdateRequest.AddWhere("NoteId", newNote.Id);
			_databaseConnection.Update<Note>(noteUpdateRequest, newNote);


			//Updates bug
			UpdateRequest updateTimeLog = new UpdateRequest("tblTimeLog");
			updateTimeLog.AddData("Name", TimeLog.Name);
			updateTimeLog.AddData("Start", TimeLog.Start);
			updateTimeLog.AddData("End", TimeLog.End);
			updateTimeLog.AddData("TaskCode", TimeLog.TaskCode);


			updateTimeLog.AddWhere("TimeLogId", TimeLog.Id);

			_databaseConnection.Update(updateTimeLog);

			response.Success = true;
			return response;
		}
	}
}
