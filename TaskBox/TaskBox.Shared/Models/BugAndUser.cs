using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class BugAndUser
	{
		public BugAndUser()
		{

		}

		public BugAndUser(Bug Bug, int UserId)
		{
			this.UserId = UserId;
			this.Bug = Bug;
		}

		public int UserId { get; set; }
		public Bug Bug { get; set; }
	}
}
