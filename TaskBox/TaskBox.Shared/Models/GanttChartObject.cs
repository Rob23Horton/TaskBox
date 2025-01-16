using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class GanttChartObject
	{
		public GanttChartObject()
		{
			this.Name = string.Empty;
			this.Description = string.Empty;
		}

		public GanttChartObject(int Id, string Name, string Description, DateTime Start, DateTime Due, TimeSpan Duration)
		{
			this.Id = Id;
			this.Name = Name;
			this.Description = Description;
			this.Start = Start;
			this.Due = Due;
			this.Duration = Duration;
		}

		public GanttChartObject(object objectItem)
		{
			if (objectItem is GanttChartObject ganttObject)
			{
				this.Id = ganttObject.Id;
				this.Name = ganttObject.Name;
				this.Description = ganttObject.Description;
				this.Start = ganttObject.Start;
				this.Due = ganttObject.Due;
				this.Duration = ganttObject.Duration;
			}
			else
			{
				throw new Exception("Gantt Chart Object must be supplied with an object that is derived from it!");
			}
		}


		public virtual int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Start { get; set; }
		public DateTime Due { get; set; }
		public TimeSpan Duration { get; set; }
	}
}
