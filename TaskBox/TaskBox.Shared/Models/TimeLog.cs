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
