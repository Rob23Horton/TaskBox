using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class UserProject
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public int UserId { get; set; }
		public int ProjectId { get; set; }
		public string Permission {  get; set; }
	}
}
