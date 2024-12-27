using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface IProjectRepository
	{
		public bool CheckProjectPermission(int UserId, int ProjectId);
		public Project GetProject(int ProjectId);
	}
}
