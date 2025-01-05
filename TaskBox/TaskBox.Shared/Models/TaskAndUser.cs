using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class TaskAndUser
	{
		public TaskAndUser()
		{
			this.Task = new TaskBoxTask();
		}

		public TaskAndUser(int UserId, TaskBoxTask Task)
		{
			this.UserId = UserId;
			this.Task = Task;
		}
		public TaskBoxTask Task { get; set; }
		public int UserId { get; set; }
	}
}
