using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class Segment : GanttChartObject
	{
		public Segment()
		{
			
		}

		public Segment(Segment Segment) : base(Segment)
		{
			this.Id = Segment.Id;
			this.OwnerProject = Segment.OwnerProject;
			this.OwnerProjectName = Segment.OwnerProjectName;
			this.BugNumber = Segment.BugNumber;
		}


		[InsertIgnore]
		public override int Id { get; set; }
		public int OwnerProject { get; set; }
		public string OwnerProjectName { get; set; } = "";
		public long BugNumber { get; set; }
	}
}
