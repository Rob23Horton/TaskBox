using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class InsertData
	{
		public InsertData(string ValueName, object Value)
		{
			this.ValueName = ValueName;
			this.Value = Value;
		}

		public string ValueName { get; set; }
		public object Value { get; set; }
	}
}
