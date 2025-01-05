using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ITaskRepository
	{
		public List<TaskBoxTask> GetTasksFromSegmentId(int SegmentId);
		public ApiResponse CreateTask(TaskBoxTask Task, int UserId);
	}
}
