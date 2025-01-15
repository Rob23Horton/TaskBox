using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class SegmentRepository : ISegmentRepository
	{
		public readonly IDatabaseConnection _databaseConnection;
		public readonly IProjectRepository _projectRepository;
		public SegmentRepository(IDatabaseConnection databaseConnection, IProjectRepository projectRepository)
		{
			this._databaseConnection = databaseConnection;
			this._projectRepository = projectRepository;
		}

		public ApiResponse CreateSegment(Segment segment, int UserId)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _projectRepository.GetProjectUserPermission(UserId, segment.OwnerProject);

			Console.WriteLine($"User {UserId} is adding {segment.Name} to project {segment.OwnerProject} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Inserts into the database
			Note note = new Note(segment.Description);
			InsertRequest noteInsert = new InsertRequest("tblNote");

			_databaseConnection.Insert<Note>(noteInsert, note);

			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", note.Description);

			int noteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;

			InsertRequest segmentInsert = new InsertRequest("tblSegment");

			segmentInsert.AddData("Name", segment.Name);
			segmentInsert.AddData("ProjectCode", segment.OwnerProject);
			segmentInsert.AddData("Start", segment.Start);
			segmentInsert.AddData("Due", segment.Due);
			segmentInsert.AddData("NoteCode", noteCode);

			_databaseConnection.Insert(segmentInsert);

			response.Success = true;
			return response;
		}

		public Segment GetSegmentFromId(int SegmentId)
		{
			SelectRequest segmentsRequest = new SelectRequest("tblSegment");

			segmentsRequest.AddData("tblSegment", "SegmentId", "Id");
			segmentsRequest.AddData("tblSegment", "Name");
			segmentsRequest.AddData("tblProject", "ProjectId", "OwnerProject");
			segmentsRequest.AddData("tblProject", "Name", "OwnerProjectName");
			segmentsRequest.AddData("tblSegment", "Start");
			segmentsRequest.AddData("tblSegment", "Due");
			segmentsRequest.AddData("tblNote", "Description");

			//Adds bugs number
			SelectRequest bugRequest = new SelectRequest("tblBug");
			bugRequest.AddData("BugId");
			bugRequest.AddWhere("SegmentCode", SegmentId);
			bugRequest.AddWhere("Completed", false);
			segmentsRequest.AddData(Functions.Count, bugRequest, "BugNumber");

			segmentsRequest.AddJoin("tblSegment", "NoteCode", "tblNote", "NoteId");
			segmentsRequest.AddJoin("tblSegment", "ProjectCode", "tblProject", "ProjectId");

			segmentsRequest.AddWhere("tblSegment", "SegmentId", SegmentId);

			List<Segment> segment = _databaseConnection.Select<Segment>(segmentsRequest);

			//Gets all durations for segment
			segment = GetDurationForSegments(segment);

			return segment[0];
		}

		private List<Segment> GetDurationForSegments(List<Segment> Segments)
		{
			foreach (Segment segment in Segments)
			{
				SelectRequest selectSegmentDuration = new SelectRequest("tblTimeLog");
				selectSegmentDuration.AddData("tblTimeLog", "TimeLogId", "ObjectId");

				SelectRequest selectDuration = new SelectRequest("tblTimeLog");
				selectDuration.AddData("End");
				selectDuration.AddData("Start");
				selectSegmentDuration.AddData(Functions.TimeDiff, selectDuration, "Duration");

				selectSegmentDuration.AddJoin("tblTimeLog", "TaskCode", "tblTask", "TaskId");
				selectSegmentDuration.AddWhere("tblTask", "SegmentCode", segment.Id);

				List<ItemDuration> durations = _databaseConnection.Select<ItemDuration>(selectSegmentDuration);

				TimeSpan total = new TimeSpan(0);
				durations.ForEach(d => total += d.Duration);

				segment.Duration = total;
			}

			return Segments;
		}

		public List<Segment> GetSegmentsFromProjectId(int ProjectId)
		{
			try
			{
				SelectRequest segmentsRequest = new SelectRequest("tblSegment");

				segmentsRequest.AddData("tblSegment", "SegmentId", "Id");
				segmentsRequest.AddData("tblSegment", "Name");
				segmentsRequest.AddData("tblSegment", "ProjectCode", "OwnerProject");
				segmentsRequest.AddData("tblSegment", "Start");
				segmentsRequest.AddData("tblSegment", "Due");
				segmentsRequest.AddData("tblNote", "Description");

				segmentsRequest.AddJoin("tblSegment", "NoteCode", "tblNote", "NoteId");

				segmentsRequest.AddWhere("tblSegment", "ProjectCode", ProjectId);

				List<Segment> segments = _databaseConnection.Select<Segment>(segmentsRequest);

				//Gets all durations for segments
				segments = GetDurationForSegments(segments);

				return segments;
			}
			catch
			{
				return new List<Segment>();
			}
		}

		public ProjectUserPermission UserSegmentPermission(int SegmentId, int UserId)
		{
			ProjectUserPermission permission = new ProjectUserPermission();

			try
			{
				SelectRequest permissionRequest = new SelectRequest("tblProjectUser");
				permissionRequest.AddData("ProjectUserId");
				permissionRequest.AddData("tblProjectUser", "ProjectUserId", "Id");
				permissionRequest.AddData("UserCode");
				permissionRequest.AddData("ProjectCode");
				permissionRequest.AddData("Permission");

				permissionRequest.AddJoin("tblProjectUser", "ProjectCode", "tblProject", "ProjectId");
				permissionRequest.AddJoin("tblProject", "ProjectId", "tblSegment", "ProjectCode");

				permissionRequest.AddWhere("UserCode", UserId);
				permissionRequest.AddWhere("tblSegment", "SegmentId", SegmentId);

				List<ProjectUserPermission> permissions = _databaseConnection.Select<ProjectUserPermission>(permissionRequest);

				if (permissions.Count() == 0)
				{
					permission.UserCode = UserId;
					permission.Permission = "N";
				}

				permission = permissions[0];
			}
			catch
			{
				permission.UserCode = UserId;
				permission.Permission = "N";
			}

			return permission;
		}
	}
}
