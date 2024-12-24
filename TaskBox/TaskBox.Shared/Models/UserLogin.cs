using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class UserLogin
	{

		public UserLogin()
		{
			this.UserName = "";
			this.Password = "";
		}

		public UserLogin(string UserName, string Password)
		{
			this.UserName = UserName;
			this.Password = Password;
		}

		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
