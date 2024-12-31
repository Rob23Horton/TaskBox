using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ISegmentRepository
	{
		public List<Segment> GetSegmentsFromProjectId(int ProjectId);
		public Task<ApiResponse> CreateSegment(Segment segment, int UserId);
	}
}
