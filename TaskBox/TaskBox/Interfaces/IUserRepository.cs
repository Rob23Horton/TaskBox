using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface IUserRepository
	{
		public User? CheckUserCredentials(string UserName, string Password);
		public ApiResponse CreateAccount(string UserName, string Password);
	}
}
