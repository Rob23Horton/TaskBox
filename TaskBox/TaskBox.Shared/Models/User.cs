using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class User
	{
		public User()
		{
			UserName = "";
		}

		public User(int Id, string UserName, string Password)
		{
			this.Id = Id;
			this.UserName = UserName;
		}

		[InsertIgnore]
		public int Id { get; set; }
		public string UserName { get; set; }
	}
}
