using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface IBugRepository
	{
		public List<Bug> GetBugsForSegment(int SegmentId);
		public ApiResponse CreateBug(Bug Bug, int UserId);
		public ApiResponse UpdateBug(Bug Bug, int UserId);
	}
}
