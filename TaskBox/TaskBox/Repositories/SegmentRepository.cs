using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class SegmentRepository : ISegmentRepository
	{
		public readonly IDatabaseConnection _databaseConnection;
		public SegmentRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
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
