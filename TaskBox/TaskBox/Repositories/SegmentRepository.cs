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

		public async Task<ApiResponse> CreateSegment(Segment segment, int UserId)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _projectRepository.GetProjectUserPermission(UserId, segment.OwnerProject);

			Console.WriteLine(userPermission.Permission.ToString());
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M")
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

				return segments;
			}
			catch
			{
				return new List<Segment>();
			}
		}
	}
}
