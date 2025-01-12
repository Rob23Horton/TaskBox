using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class CheckboxRepository : ICheckboxRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		private readonly ITaskRepository _taskRepository;
		public CheckboxRepository(IDatabaseConnection databaseConnection, ITaskRepository taskRepository)
		{
			this._databaseConnection = databaseConnection;
			this._taskRepository = taskRepository;
		}

		public ApiResponse CreateCheckbox(int UserId, Checkbox Checkbox)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _taskRepository.CheckPermission(UserId, Checkbox.TaskCode);

			Console.WriteLine($"User {UserId} is adding {Checkbox.Name} to project {Checkbox.TaskCode} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S" && userPermission.Permission.ToUpper() != "T")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			InsertRequest checkboxInsert = new InsertRequest("tblCheckbox");

			_databaseConnection.Insert<Checkbox>(checkboxInsert, Checkbox);

			response.Success = true;
			return response;
		}

		public List<Checkbox> GetCheckboxesFromTaskId(int TaskId)
		{
			SelectRequest checkboxRequest = new SelectRequest("tblCheckbox");

			checkboxRequest.AddData("tblCheckbox", "CheckboxId", "Id");
			checkboxRequest.AddData("Name");
			checkboxRequest.AddData("Checked");
			checkboxRequest.AddData("TaskCode");

			checkboxRequest.AddWhere("TaskCode", TaskId);

			List<Checkbox> checkboxes = _databaseConnection.Select<Checkbox>(checkboxRequest);

			return checkboxes;
		}

		public ApiResponse UpdateCheckbox(int UserId, Checkbox Checkbox)
		{
			ApiResponse response = new ApiResponse();

			//Check user has permission to create segments (A or M)
			ProjectUserPermission userPermission = _taskRepository.CheckPermission(UserId, Checkbox.TaskCode);

			Console.WriteLine($"User {UserId} is adding {Checkbox.Name} to project {Checkbox.TaskCode} with permission {userPermission.Permission}!");
			if (userPermission.Permission.ToUpper() != "A" && userPermission.Permission.ToUpper() != "M" && userPermission.Permission.ToUpper() != "S" && userPermission.Permission.ToUpper() != "T")
			{
				response.Success = false;
				response.Message = "User doesn't have the permissions for this";
				return response;
			}

			SelectRequest updateCheckbox = new SelectRequest("tblCheckbox");

			updateCheckbox.AddWhere("CheckboxId", Checkbox.Id);

			_databaseConnection.Update<Checkbox>(updateCheckbox, Checkbox);

			response.Success = true;
			return response;
		}
	}
}
