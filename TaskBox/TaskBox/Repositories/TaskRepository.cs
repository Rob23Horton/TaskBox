using DatabaseConnection.Models;
using DatabaseConnection.Services;
using System.Security;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		private readonly ISegmentRepository _segmentRepository;
		public TaskRepository(IDatabaseConnection databaseConnection, ISegmentRepository segmentRepository)
		{
			this._databaseConnection = databaseConnection;
			this._segmentRepository = segmentRepository;
		}

		public ProjectUserPermission CheckPermission(int UserId, int TaskId)
		{
			SelectRequest permissionRequest = new SelectRequest("tblProjectUser");
			permissionRequest.AddData("ProjectUserId");
			permissionRequest.AddData("tblProjectUser", "ProjectUserId", "Id");
			permissionRequest.AddData("UserCode");
			permissionRequest.AddData("ProjectCode");
			permissionRequest.AddData("Permission");

			permissionRequest.AddJoin("tblProjectUser", "ProjectCode", "tblProject", "ProjectId");
			permissionRequest.AddJoin("tblProject", "ProjectId", "tblSegment", "ProjectCode");
			permissionRequest.AddJoin("tblSegment", "SegmentId", "tblTask", "SegmentCode");

			permissionRequest.AddWhere("UserCode", UserId);
			permissionRequest.AddWhere("tblTask", "TaskId", TaskId);

			List<ProjectUserPermission> permissions = _databaseConnection.Select<ProjectUserPermission>(permissionRequest);

			if (permissions.Count() == 0)
			{
				ProjectUserPermission permission = new ProjectUserPermission();
				permission.UserCode = UserId;
				permission.Permission = "N";
			}

			return permissions[0];
		}

		public ApiResponse CreateTask(TaskBoxTask Task, int UserId)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _segmentRepository.UserSegmentPermission(Task.SegmentCode, UserId);

			Console.WriteLine($"User {UserId} is adding {Task.Name} to segment {Task.SegmentCode} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S" && userPermission.Permission.ToUpper() != "T")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Inserts into the database
			Note note = new Note(Task.Description);
			InsertRequest noteInsert = new InsertRequest("tblNote");

			_databaseConnection.Insert<Note>(noteInsert, note);

			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", note.Description);

			int noteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;

			InsertRequest taskInsert = new InsertRequest("tblTask");

			taskInsert.AddData("Name", Task.Name);
			taskInsert.AddData("SegmentCode", Task.SegmentCode);
			taskInsert.AddData("Start", Task.Start);
			taskInsert.AddData("Due", Task.Due);
			taskInsert.AddData("NoteCode", noteCode);

			_databaseConnection.Insert(taskInsert);

			response.Success = true;
			return response;
		}

		public TaskBoxTask GetTask(int TaskId)
		{
			SelectRequest taskRequest = new SelectRequest("tblTask");

			taskRequest.AddData("tblTask", "TaskId", "Id");
			taskRequest.AddData("tblTask", "Name");
			taskRequest.AddData("tblSegment", "SegmentId", "SegmentCode");
			taskRequest.AddData("tblSegment", "Name", "SegmentName");
			taskRequest.AddData("tblProject", "ProjectId", "ProjectCode");
			taskRequest.AddData("tblProject", "Name", "ProjectName");
			taskRequest.AddData("tblTask", "Start");
			taskRequest.AddData("tblTask", "Due");
			taskRequest.AddData("tblNote", "Description");

			taskRequest.AddJoin("tblTask", "NoteCode", "tblNote", "NoteId");
			taskRequest.AddJoin("tblTask", "SegmentCode", "tblSegment", "SegmentId");
			taskRequest.AddJoin("tblSegment", "ProjectCode", "tblProject", "ProjectId");

			taskRequest.AddWhere("tblTask", "TaskId", TaskId);

			List<TaskBoxTask> task = _databaseConnection.Select<TaskBoxTask>(taskRequest);

			return task[0];
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
