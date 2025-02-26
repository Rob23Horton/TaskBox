﻿using DatabaseConnection.Models;
using DatabaseConnection.Services;
using System.Security;
using System.Threading.Tasks;
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
				return permission;
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

		private List<TaskBoxTask> GetDurationForTasks(List<TaskBoxTask> Tasks)
		{
			foreach (TaskBoxTask task in Tasks)
			{
				SelectRequest selectTaskDuration = new SelectRequest("tblTimeLog");
				selectTaskDuration.AddData("tblTimeLog", "TimeLogId", "ObjectId");

				SelectRequest selectDuration = new SelectRequest("tblTimeLog");
				selectDuration.AddData("End");
				selectDuration.AddData("Start");
				selectTaskDuration.AddData(Functions.TimeDiff, selectDuration, "Duration");

				selectTaskDuration.AddWhere("TaskCode", task.Id);

				List<ItemDuration> durations = _databaseConnection.Select<ItemDuration>(selectTaskDuration);

				TimeSpan total = new TimeSpan(0);
				durations.ForEach(d => total += d.Duration);

				task.Duration = total;
			}

			return Tasks;
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

			task = GetDurationForTasks(task);

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

				tasks = GetDurationForTasks(tasks);

				return tasks;
			}
			catch
			{
				return new List<TaskBoxTask>();
			}
		}

		public ApiResponse UpdateTask(int UserId, TaskBoxTask Task)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _segmentRepository.UserSegmentPermission(Task.SegmentCode, UserId);
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Gets Parent Project Start/End Dates
			Segment parentSegment = _segmentRepository.GetSegmentFromId(Task.SegmentCode);

			if (Task.Start < parentSegment.Start)
			{
				response.Success = false;
				response.Message = "Task start date cannot be before segment date!";
				return response;
			}
			else if (Task.Due > parentSegment.Due)
			{
				response.Success = false;
				response.Message = "Task end date cannot be after segment date!";
				return response;
			}

			//Gets note code
			SelectRequest noteRequest = new SelectRequest("tblNote");
			noteRequest.AddData("tblNote", "NoteId", "Id");
			noteRequest.AddJoin("tblNote", "NoteId", "tblTask", "NoteCode");
			noteRequest.AddWhere("tblTask", "TaskId", Task.Id);
			Note note = _databaseConnection.Select<Note>(noteRequest)[0];

			//Updates Note
			note.Description = Task.Description;
			UpdateRequest noteUpdate = new UpdateRequest("tblNote");
			noteUpdate.AddWhere("NoteId", note.Id);

			_databaseConnection.Update<Note>(noteUpdate, note);


			//Updates Segment
			UpdateRequest taskInsert = new UpdateRequest("tblTask");
			taskInsert.AddData("Name", Task.Name);
			taskInsert.AddData("Start", Task.Start);
			taskInsert.AddData("Due", Task.Due);

			taskInsert.AddWhere("TaskId", Task.Id);

			_databaseConnection.Update(taskInsert);

			response.Success = true;
			return response;
		}
	}
}
