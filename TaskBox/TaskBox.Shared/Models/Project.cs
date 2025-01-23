using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class Project : GanttChartObject
	{
		public Project()
		{
			
		}

		public Project(Project Project) : base(Project)
		{
			this.Id = Project.Id;
		}

		[InsertIgnore]
		public override int Id { get; set; }
	}
}
