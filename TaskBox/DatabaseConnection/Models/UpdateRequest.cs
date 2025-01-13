using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class UpdateRequest
	{
		public UpdateRequest(string Table)
		{
			this.Table = Table;
		}

		public void AddData(string ValueName, object Value)
		{
			KeyValuePair<string, object> newValue = new KeyValuePair<string, object>(ValueName, Value);
			Values.Add(newValue);
		}

		public void AddWhere(RequestWhere requestWhere)
		{
			WhereData.Add(requestWhere);
		}
		public void AddWhere(string ValueName, object Value)
		{
			RequestWhere newWhere = new RequestWhere(this.Table, ValueName, Value);
			AddWhere(newWhere);
		}

		public string Table { get; set; }
		public List<KeyValuePair<string, object>> Values { get; set; } = new List<KeyValuePair<string, object>>();
		public List<RequestWhere> WhereData { get; set; } = new List<RequestWhere>();
	}
}
