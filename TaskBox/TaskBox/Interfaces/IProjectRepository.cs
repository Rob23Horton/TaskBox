namespace TaskBox.Interfaces
{
	public interface IProjectRepository
	{
		public bool CheckProjectPermission(int UserId, int ProjectId);
	}
}
