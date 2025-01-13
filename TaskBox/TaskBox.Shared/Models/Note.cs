using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class Note
	{
		public Note()
		{
			Description = "";
		}
		public Note(int id, string description)
		{
			Id = id;
			Description = description;
		}

		public Note(string Description)
		{
			this.Description = Description;
		}

		[InsertIgnore]
		public int Id { get; set; }
		public string Description { get; set; }
	}
}
