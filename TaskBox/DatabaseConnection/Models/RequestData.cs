using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class RequestData
	{
		public RequestData(string Table, string ValueName, object Value)
		{
			this.Table = Table;
			this.ValueName = ValueName;
			this.Value = Value;
			this.ParseTo = ValueName;
		}

		public RequestData(string Table, string ValueName, object Value, string As)
		{
			this.Table = Table;
			this.ValueName = ValueName;
			this.Value = Value;
			this.ParseTo = As;
		}

		public string Table { get; set; }
		public string ParseTo { get; set; }
		public string ValueName { get; set; }
		public object Value { get; set; }
	}
}
