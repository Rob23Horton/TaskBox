using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class TaskBoxTask : GanttChartObject
	{
		public TaskBoxTask()
		{
			
		}

		public TaskBoxTask(TaskBoxTask Task) : base(Task)
		{
			this.Id = Task.Id;
			this.ProjectCode = Task.ProjectCode;
			this.ProjectName = Task.ProjectName;
			this.SegmentCode = Task.SegmentCode;
			this.SegmentName = Task.SegmentName;
		}

		[InsertIgnore]
		public override int Id { get; set; }
		public int SegmentCode { get; set; }
		public int ProjectCode { get; set; }
		public string ProjectName { get; set; } = "";
		public string SegmentName { get; set; } = "";
	}
}
