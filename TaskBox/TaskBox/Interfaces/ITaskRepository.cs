using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ITaskRepository
	{
		public TaskBoxTask GetTask(int TaskId);
		public ProjectUserPermission CheckPermission(int UserId, int TaskId);
		public List<TaskBoxTask> GetTasksFromSegmentId(int SegmentId);
		public ApiResponse CreateTask(TaskBoxTask Task, int UserId);
	}
}
