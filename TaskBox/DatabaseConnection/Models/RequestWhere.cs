using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class RequestWhere
	{
		public string Table { get; set; }
		public string ValueName { get; set; }
		public object Value { get; set; }
	}
}
