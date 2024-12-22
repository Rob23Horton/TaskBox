using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class InsertRequest
	{
		public InsertRequest(string Table)
		{
			this.Table = Table;
		}

		public void AddData(InsertData insertData)
		{
			this.Data.Add(insertData);
		}
		public void AddData(string ValueName, object Value)
		{
			InsertData newData = new InsertData(ValueName, Value);
			AddData(newData);
		}


		public string Table { get; set; }
		public List<InsertData> Data { get; set; } = new List<InsertData>();
	}
}
