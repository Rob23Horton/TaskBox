using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class TimeLog
	{
		public TimeLog()
		{
			
		}

		public TimeLog(TimeLog TimeLog)
		{
			this.Id = TimeLog.Id;
			this.Name = TimeLog.Name;
			this.TaskCode = TimeLog.TaskCode;
			this.TaskName = TimeLog.TaskName;
			this.Start = TimeLog.Start;
			this.End = TimeLog.End;
			this.Description = TimeLog.Description;
		}

		[InsertIgnore]
		public int Id { get; set; }
		public string Name { get; set; }
		public int TaskCode { get; set; }
		public string TaskName { get; set; } = "";
		public int UserCode { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Description { get; set; }
	}
}
