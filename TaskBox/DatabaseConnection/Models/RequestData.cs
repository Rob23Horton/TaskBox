using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class RequestData
	{
		public RequestData(string Table, string ValueName)
		{
			this.Table = Table;
			this.ValueName = ValueName;
			this.ParseTo = ValueName;
		}

		public RequestData(string Table, string ValueName, string As)
		{
			this.Table = Table;
			this.ValueName = ValueName;
			this.ParseTo = As;
		}

		public string Table { get; set; }
		public string ParseTo { get; set; }
		public string ValueName { get; set; }
	}
}
