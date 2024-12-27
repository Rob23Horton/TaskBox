using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class Segment
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int OwnerProject { get; set; }
		public DateTime Start { get; set; }
		public DateTime Due { get; set; }
		public string Description { get; set; }
	}
}
