using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ILogTimeRepository
	{
		public List<TimeLog> GetAllTimeLogsForUser(int UserId);
	}
}
