using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class LogTimeRepository : ILogTimeRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public LogTimeRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
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
	}
}
