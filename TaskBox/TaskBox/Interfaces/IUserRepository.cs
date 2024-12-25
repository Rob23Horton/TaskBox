using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface IUserRepository
	{
		public User? CheckUserCredentials(string UserName, string Password);
	}
}
