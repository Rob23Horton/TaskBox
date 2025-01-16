using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface IProjectRepository
	{
		public ApiResponse CreateProject(int UserId, Project Project);
		public bool CheckProjectPermission(int UserId, int ProjectId);
		public ProjectUserPermission GetProjectUserPermission(int UserId, int ProjectId);
		public Project GetProject(int ProjectId);
		public List<Project> GetUserProjects(int UserId);
	}
}
