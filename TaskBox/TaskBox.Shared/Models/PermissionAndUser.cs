using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class PermissionAndUser
	{
		public PermissionAndUser()
		{
			
		}

		public PermissionAndUser(ProjectUserPermission Permission, int UserId)
		{
			this.Permission = Permission;
			this.UserId = UserId;
		}

		public int UserId { get; set; }
		public ProjectUserPermission Permission { get; set; }
	}
}
