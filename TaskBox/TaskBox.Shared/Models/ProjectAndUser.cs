using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class ProjectAndUser
	{
		public ProjectAndUser()
		{
			
		}

		public ProjectAndUser(int UserId, Project Project)
		{
			this.UserId = UserId;
			this.Project = Project;
		}
		public int UserId { get; set; }
		public Project Project { get; set; }
	}
}
