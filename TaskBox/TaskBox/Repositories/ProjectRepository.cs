﻿using DatabaseConnection.Models;
using DatabaseConnection.Services;
using System.Security;
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

		public List<ProjectUserPermission> GetPermissionsForProject(int ProjectId)
		{
			SelectRequest permissionsRequest = new SelectRequest("tblProjectUser");

			permissionsRequest.AddData("tblProjectUser", "ProjectUserId", "Id");
			permissionsRequest.AddData("tblUser", "UserName");
			permissionsRequest.AddData("UserCode");
			permissionsRequest.AddData("ProjectCode");
			permissionsRequest.AddData("Permission");

			permissionsRequest.AddJoin("tblProjectUser", "UserCode", "tblUser", "UserId");

			permissionsRequest.AddWhere("ProjectCode", ProjectId);

			List<ProjectUserPermission> permissions = _databaseConnection.Select<ProjectUserPermission>(permissionsRequest);

			return permissions;
		}

		public ApiResponse CreateUserPermission(int UserId, ProjectUserPermission Permission)
		{
			ApiResponse response = new ApiResponse();

			ProjectUserPermission creatorUserPermission = GetProjectUserPermission(UserId, Permission.ProjectCode);
			
			if (creatorUserPermission.Permission.ToUpper() != "A")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			ProjectUserPermission newUserPermission = GetProjectUserPermission(Permission.UserCode, Permission.ProjectCode);

			if (newUserPermission.Permission.ToUpper() != "N")
			{
				response.Success = false;
				response.Message = "New User is already has permissions for this project";
				return response;
			}

			InsertRequest permissionInsert = new InsertRequest("tblProjectUser");

			_databaseConnection.Insert<ProjectUserPermission>(permissionInsert, Permission);

			response.Success = true;
			return response;
		}

		public ApiResponse UpdateUserPermission(int UserId, ProjectUserPermission Permission)
		{
			ApiResponse response = new ApiResponse();

			ProjectUserPermission creatorUserPermission = GetProjectUserPermission(UserId, Permission.ProjectCode);

			if (creatorUserPermission.Permission.ToUpper() != "A")
			{
				response.Success = false;
				response.Message = "You doesn't have the permissions for this";
				return response;
			}

			ProjectUserPermission newUserPermission = GetProjectUserPermission(Permission.UserCode, Permission.ProjectCode);

			if (newUserPermission.Permission.ToUpper() == "N")
			{
				response.Success = false;
				response.Message = "User doesn't have any permissions for this project.";
				return response;
			}

			if (Permission.Permission == "N")
			{
				//Deletes the permission due to the user selecting the user to have permission level of 'NONE'
				DeleteRequest deletePermission = new DeleteRequest("tblProjectUser", "ProjectUserId", Permission.Id);

				_databaseConnection.Delete(deletePermission);
			}
			else
			{
				//Only allows to change the permission value
				UpdateRequest updatePermission = new UpdateRequest("tblProjectUser");
				updatePermission.AddData("Permission", Permission.Permission);

				updatePermission.AddWhere("ProjectUserId", Permission.Id);

				_databaseConnection.Update(updatePermission);
			}

			response.Success = true;
			return response;
		}

		public ApiResponse UpdateProject(int UserId, Project Project)
		{
			ApiResponse response = new ApiResponse();

			ProjectUserPermission userPermission = GetProjectUserPermission(UserId, Project.Id);
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M")
			{
				response.Success = false;
				response.Message = "User doesn't have any permissions for this project.";
				return response;
			}

			//Gets note code
			SelectRequest noteRequest = new SelectRequest("tblNote");
			noteRequest.AddData("tblNote", "NoteId", "Id");
			noteRequest.AddJoin("tblNote", "NoteId", "tblProject", "NoteCode");
			noteRequest.AddWhere("tblProject", "ProjectId", Project.Id);
			Note note = _databaseConnection.Select<Note>(noteRequest)[0];

			//Updates Note
			note.Description = Project.Description;
			UpdateRequest noteInsert = new UpdateRequest("tblNote");
			noteInsert.AddWhere("NoteId", note.Id);

			_databaseConnection.Update<Note>(noteInsert, note);


			//Updates Project
			UpdateRequest projectInsert = new UpdateRequest("tblProject");
			projectInsert.AddData("Name", Project.Name);
			projectInsert.AddData("Start", Project.Start);
			projectInsert.AddData("Due", Project.Due);

			projectInsert.AddWhere("ProjectId", Project.Id);

			_databaseConnection.Update(projectInsert);

			response.Success = true;
			return response;
		}
	}
}
