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
		[InsertIgnore]
		public int Id { get; set; }
		public int UserCode { get; set; }
		public int ProjectCode { get; set; }
		public string Permission { get; set; }
	}
}
