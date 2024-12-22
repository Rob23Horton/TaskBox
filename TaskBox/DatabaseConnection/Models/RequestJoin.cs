using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class RequestJoin
	{
		public RequestJoin(string OriginTable, string OriginValue, string ConnectorTable, string ConnectorValue)
		{
			this.OriginTable = OriginTable;
			this.OriginValue = OriginValue;
			this.ConnectorTable = ConnectorTable;
			this.ConnectorValue = ConnectorValue;
		}

		public string OriginTable { get; set; }
		public string OriginValue { get; set; }
		public string ConnectorTable { get; set; }
		public string ConnectorValue { get; set; }
	}
}
