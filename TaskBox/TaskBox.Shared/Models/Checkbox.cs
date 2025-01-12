using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class Checkbox
	{
		public Checkbox()
		{
			
		}

		public Checkbox(Checkbox Checkbox)
		{
			this.Id = Checkbox.Id;
			this.Name = Checkbox.Name;
			this.Checked = Checkbox.Checked;
			this.TaskCode = Checkbox.TaskCode;
		}

		[InsertIgnore]
		public int Id { get; set; }
		public string Name { get; set; }
		public int TaskCode { get; set; }
		public bool Checked { get; set; }
	}
}
