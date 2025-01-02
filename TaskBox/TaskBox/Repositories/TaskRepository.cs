using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public TaskRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
		}

		public List<TaskBoxTask> GetTasksFromSegmentId(int SegmentId)
		{
			try
			{
				SelectRequest tasksRequest = new SelectRequest("tblTask");

				tasksRequest.AddData("tblTask", "TaskId", "Id");
				tasksRequest.AddData("tblTask", "Name");
				tasksRequest.AddData("tblTask", "SegmentCode");
				tasksRequest.AddData("tblTask", "Start");
				tasksRequest.AddData("tblTask", "Due");
				tasksRequest.AddData("tblNote", "Description");

				tasksRequest.AddJoin("tblTask", "NoteCode", "tblNote", "NoteId");

				tasksRequest.AddWhere("tblTask", "SegmentCode", SegmentId);

				List<TaskBoxTask> tasks = _databaseConnection.Select<TaskBoxTask>(tasksRequest);

				if (tasks.Count() == 0)
				{
					return new List<TaskBoxTask>();
				}

				return tasks;
			}
			catch
			{
				return new List<TaskBoxTask>();
			}
		}
	}
}
