using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public UserRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
		}

		public User? CheckUserCredentials(string UserName, string Password)
		{
			SelectRequest userRequest = new SelectRequest("tblUser");
			userRequest.AddData("tblUser", "UserId", "Id");
			userRequest.AddData("UserName");

			userRequest.AddWhere("UserName", UserName);
			userRequest.AddWhere("Password", Password);

			List<User> users = _databaseConnection.Select<User>(userRequest);

			if (users.Count() == 0)
			{
				return null;
			}

			return users[0];
		}

		private bool UserExists(string UserName)
		{
			SelectRequest userRequest = new SelectRequest("tblUser");
			userRequest.AddData("UserName");

			userRequest.AddWhere("UserName", UserName);

			List<User> user = _databaseConnection.Select<User>(userRequest);

			if (user.Count() == 0)
			{
				return false;
			}

			return true;

		}


		public ApiResponse CreateAccount(string UserName, string Password)
		{
			ApiResponse response = new ApiResponse();

			if (UserExists(UserName))
			{
				response.Success = false;
				response.Message = "User Already Exists!";
				return response;
			}

			try
			{
				InsertRequest userInsert = new InsertRequest("tblUser");

				UserLogin newUser = new UserLogin(UserName, Password);
				_databaseConnection.Insert<UserLogin>(userInsert, newUser);

				response.Success = true;
				response.Message = "Account created successfully!";
			}
			catch
			{
				response.Success = false;
				response.Message = "An error occured. Please try again later!";
			}


			return response;
		}
	}
}
