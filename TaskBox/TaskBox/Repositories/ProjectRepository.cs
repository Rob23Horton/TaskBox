using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public ProjectRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
		}

		public bool CheckProjectPermission(int UserId, int ProjectId)
		{
			try
			{
				SelectRequest permissionRequest = new SelectRequest("tblProjectUser");
				permissionRequest.AddData("ProjectUserId");
				permissionRequest.AddData("tblProjectUser", "ProjectUserId", "Id");
				permissionRequest.AddData("UserCode");
				permissionRequest.AddData("ProjectCode");
				permissionRequest.AddData("Permission");

				permissionRequest.AddWhere("UserCode", UserId);
				permissionRequest.AddWhere("ProjectCode", ProjectId);

				List<ProjectUserPermission> permission = _databaseConnection.Select<ProjectUserPermission>(permissionRequest);

				if (permission.Count() == 0)
				{
					return false;
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public ProjectUserPermission GetProjectUserPermission(int UserId, int ProjectId)
		{
			try
			{
				SelectRequest permissionRequest = new SelectRequest("tblProjectUser");
				permissionRequest.AddData("tblProjectUser", "ProjectUserId", "Id");
				permissionRequest.AddData("ProjectCode");
				permissionRequest.AddData("UserCode");
				permissionRequest.AddData("Permission");

				permissionRequest.AddWhere("UserCode", UserId);
				permissionRequest.AddWhere("ProjectCode", ProjectId);

				List<ProjectUserPermission> permission = _databaseConnection.Select<ProjectUserPermission>(permissionRequest);

				if (permission.Count() == 0)
				{
					throw new Exception();
				}

				return permission[0];
			}
			catch
			{
				ProjectUserPermission permission = new ProjectUserPermission();
				permission.Permission = "N";
				permission.ProjectCode = ProjectId;
				permission.UserCode = UserId;

				return permission;
			}
		}

		private List<Project> GetDurationForProjects(List<Project> Projects)
		{
			foreach (Project project in Projects)
			{
				SelectRequest selectProjectDuration = new SelectRequest("tblTimeLog");
				selectProjectDuration.AddData("tblTimeLog", "TimeLogId", "ObjectId");

				SelectRequest selectDuration = new SelectRequest("tblTimeLog");
				selectDuration.AddData("End");
				selectDuration.AddData("Start");
				selectProjectDuration.AddData(Functions.TimeDiff, selectDuration, "Duration");

				selectProjectDuration.AddJoin("tblTimeLog", "TaskCode", "tblTask", "TaskId");
				selectProjectDuration.AddJoin("tblTask", "SegmentCode", "tblSegment", "SegmentId");
				selectProjectDuration.AddWhere("tblSegment", "ProjectCode", project.Id);

				List<ItemDuration> durations = _databaseConnection.Select<ItemDuration>(selectProjectDuration);

				TimeSpan total = new TimeSpan(0);
				durations.ForEach(d => total += d.Duration);

				project.Duration = total;
			}

			return Projects;
		}
		public Project GetProject(int ProjectId)
		{
			SelectRequest projectRequest = new SelectRequest("tblProject");
			projectRequest.AddData("tblProject", "ProjectId", "Id");
			projectRequest.AddData("tblProject", "Name");
			projectRequest.AddData("tblProject", "Start");
			projectRequest.AddData("tblProject", "Due");
			projectRequest.AddData("tblNote", "Description");

			projectRequest.AddJoin("tblProject", "NoteCode", "tblNote", "NoteId");

			projectRequest.AddWhere("tblProject", "ProjectId", ProjectId);

			List<Project> project = _databaseConnection.Select<Project>(projectRequest);

			if (project.Count() == 0)
			{
				throw new Exception();
			}

			project = GetDurationForProjects(project);

			return project[0];
		}

		public List<Project> GetUserProjects(int UserId)
		{
			SelectRequest projectRequest = new SelectRequest("tblProject");
			projectRequest.AddData("tblProject", "ProjectId", "Id");
			projectRequest.AddData("tblProject", "Name");
			projectRequest.AddData("tblProject", "Start");
			projectRequest.AddData("tblProject", "Due");
			projectRequest.AddData("tblNote", "Description");

			projectRequest.AddJoin("tblProject", "NoteCode", "tblNote", "NoteId");
			projectRequest.AddJoin("tblProject", "ProjectId", "tblProjectUser", "ProjectCode");

			projectRequest.AddWhere("tblProjectUser", "UserCode", UserId);

			List<Project> projects = _databaseConnection.Select<Project>(projectRequest);

			projects = GetDurationForProjects(projects);

			return projects;
		}

		public ApiResponse CreateProject(int UserId, Project Project)
		{
			ApiResponse response = new ApiResponse();

			//Inserts Note
			Note note = new Note(Project.Description);
			InsertRequest noteInsert = new InsertRequest("tblNote");

			_databaseConnection.Insert<Note>(noteInsert, note);

			//Gets Note Id
			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", note.Description);

			int noteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;

			//Inserts Project
			InsertRequest projectInsert = new InsertRequest("tblProject");
			projectInsert.AddData("Name", Project.Name);
			projectInsert.AddData("Start", Project.Start);
			projectInsert.AddData("Due", Project.Due);
			projectInsert.AddData("NoteCode", noteCode);

			_databaseConnection.Insert(projectInsert);

			//Gets Project Id
			SelectRequest projectRequest = new SelectRequest("tblProject");
			projectRequest.AddData("tblProject", "ProjectId", "Id");
			projectRequest.AddWhere("tblProject", "Name", Project.Name);
			projectRequest.AddWhere("tblProject", "Start", Project.Start);
			projectRequest.AddWhere("tblProject", "Due", Project.Due);

			List<Project> project = _databaseConnection.Select<Project>(projectRequest);

			int projectId = project[0].Id;

			//Inserts Project Permission
			InsertRequest permissionInsert = new InsertRequest("tblProjectUser");
			permissionInsert.AddData("UserCode", UserId);
			permissionInsert.AddData("ProjectCode", projectId);
			permissionInsert.AddData("Permission", "A");

			_databaseConnection.Insert(permissionInsert);

			response.Success = true;
			return response;
		}
	}
}
