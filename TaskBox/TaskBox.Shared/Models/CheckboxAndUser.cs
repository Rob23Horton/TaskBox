using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class CheckboxAndUser
	{
		public CheckboxAndUser()
		{

		}

		public CheckboxAndUser(int userId, Checkbox checkbox)
		{
			UserId = userId;
			Checkbox = checkbox;
		}

		public int UserId { get; set; }
		public Checkbox Checkbox { get; set; }
	}
}
