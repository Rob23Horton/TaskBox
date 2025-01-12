using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ICheckboxRepository
	{
		public List<Checkbox> GetCheckboxesFromTaskId(int TaskId);
		public ApiResponse CreateCheckbox(int UserId, Checkbox Checkbox);
		public ApiResponse UpdateCheckbox(int UserId, Checkbox Checkbox);

	}
}
