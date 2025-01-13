using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class Bug
	{
		public Bug()
		{
			
		}

		public Bug(Bug bug)
		{
			this.Id = bug.Id;
			this.Name = bug.Name;
			this.SegmentCode = bug.SegmentCode;
			this.CreatedDate = bug.CreatedDate;
			this.Completed = bug.Completed;
			this.Description = bug.Description;
		}

		[InsertIgnore]
		public int Id { get; set; }
		public string Name { get; set; }
		public int SegmentCode { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool Completed { get; set; }
		public string Description { get; set; }
	}
}
