﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class User
	{
		public User()
		{
			UserName = "";
			Password = "";
		}

		public User(int Id, string UserName, string Password)
		{
			this.Id = Id;
			this.UserName = UserName;
			this.Password = Password;
		}

		public int Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
