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
	}
}
