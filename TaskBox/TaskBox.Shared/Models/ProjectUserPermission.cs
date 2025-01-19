using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class ProjectUserPermission
	{
		public ProjectUserPermission()
		{
			
		}

		public ProjectUserPermission(ProjectUserPermission Permission)
		{
			this.Id = Permission.Id;
			this.UserName = Permission.UserName;
			this.UserCode = Permission.UserCode;
			this.ProjectCode = Permission.ProjectCode;
			this.Permission = Permission.Permission;
		}

		[InsertIgnore]
		public int Id { get; set; }
		[InsertIgnore]
		public string UserName { get; set; } = "";
		public int UserCode { get; set; }
		public int ProjectCode { get; set; }
		public string Permission { get; set; }
	}
}
