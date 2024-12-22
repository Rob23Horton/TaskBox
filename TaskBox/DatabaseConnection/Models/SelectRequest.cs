using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class SelectRequest
	{
		public SelectRequest(string Table)
		{
			this.Table = Table;
		}

		public string Table { get; set; }
		public List<RequestData> Data { get; set; } = new List<RequestData>();
		public List<RequestJoin> Joins { get; set; } = new List<RequestJoin>();
		public List<RequestWhere> WhereData { get; set; } = new List<RequestWhere>();
	}
}
