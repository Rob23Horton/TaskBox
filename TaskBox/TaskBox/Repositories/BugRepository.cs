using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class BugRepository : IBugRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		private readonly ISegmentRepository _segmentRepository;
		public BugRepository(IDatabaseConnection databaseConnection, ISegmentRepository segmentRepository)
		{
			this._databaseConnection = databaseConnection;
			_segmentRepository = segmentRepository;
		}

		public ApiResponse CreateBug(Bug Bug, int UserId)
		{
			ApiResponse response = new ApiResponse();

			ProjectUserPermission userPermission = _segmentRepository.UserSegmentPermission(Bug.SegmentCode, UserId);

			Console.WriteLine($"User {UserId} is adding bug {Bug.Name} to segment {Bug.SegmentCode} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Inserts into the database
			Note note = new Note(Bug.Description);
			InsertRequest noteInsert = new InsertRequest("tblNote");

			_databaseConnection.Insert<Note>(noteInsert, note);

			SelectRequest noteSelect = new SelectRequest("tblNote");
			noteSelect.AddData("tblNote", "NoteId", "Id");
			noteSelect.AddWhere("Description", note.Description);

			int noteCode = _databaseConnection.Select<Note>(noteSelect)[0].Id;

			InsertRequest bugInsert = new InsertRequest("tblBug");

			bugInsert.AddData("Name", Bug.Name);
			bugInsert.AddData("SegmentCode", Bug.SegmentCode);
			bugInsert.AddData("CreatedDate", Bug.CreatedDate);
			bugInsert.AddData("Completed", Bug.Completed);
			bugInsert.AddData("NoteCode", noteCode);

			_databaseConnection.Insert(bugInsert);

			response.Success = true;
			return response;
		}

		public List<Bug> GetBugsForSegment(int SegmentId)
		{
			SelectRequest bugRequest = new SelectRequest("tblBug");

			bugRequest.AddData("tblBug", "BugId", "Id");
			bugRequest.AddData("Name");
			bugRequest.AddData("SegmentCode");
			bugRequest.AddData("CreatedDate");
			bugRequest.AddData("Completed");
			bugRequest.AddData("tblNote", "Description");

			bugRequest.AddJoin("tblBug", "NoteCode", "tblNote", "NoteId");

			bugRequest.AddWhere("SegmentCode", SegmentId);

			List<Bug> bugs = _databaseConnection.Select<Bug>(bugRequest);

			return bugs;
		}

		public ApiResponse UpdateBug(Bug Bug, int UserId)
		{
			ApiResponse response = new ApiResponse();

			ProjectUserPermission userPermission = _segmentRepository.UserSegmentPermission(Bug.SegmentCode, UserId);

			Console.WriteLine($"User {UserId} is adding bug {Bug.Name} to segment {Bug.SegmentCode} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			//Gets note id
			SelectRequest previouseBugRequest = new SelectRequest("tblBug");
			previouseBugRequest.AddData("tblBug", "NoteCode", "Id");
			previouseBugRequest.AddWhere("BugId", Bug.Id);
			List<Note> pastNote = _databaseConnection.Select<Note>(previouseBugRequest);

			//Creates and updates note
			Note newNote = new Note(pastNote[0].Id, Bug.Description);
			UpdateRequest noteUpdateRequest = new UpdateRequest("tblNote");
			noteUpdateRequest.AddWhere("NoteId", newNote.Id);
			_databaseConnection.Update<Note>(noteUpdateRequest, newNote);


			//Updates bug
			UpdateRequest updateBug = new UpdateRequest("tblBug");
			updateBug.AddData("Name", Bug.Name);
			updateBug.AddData("Completed", Bug.Completed);

			updateBug.AddWhere("BugId", Bug.Id);

			_databaseConnection.Update(updateBug);


			response.Success = true;
			return response;
		}
	}
}
