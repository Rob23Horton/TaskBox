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

		public void AddData(RequestData requestData)
		{
			this.Data.Add(requestData);
		}
		public void AddData(string ValueName)
		{
			RequestData newData = new RequestData(this.Table, ValueName);
			AddData(newData);
		}
		public void AddData(string Table, string ValueName)
		{
			RequestData newData = new RequestData(Table, ValueName);
			AddData(newData);
		}
		public void AddData(string Table, string ValueName, string As)
		{
			RequestData newData = new RequestData(Table, ValueName, As);
			AddData(newData);
		}

		
		public void AddJoin(RequestJoin requestJoin)
		{
			this.Joins.Add(requestJoin);
		}
		public void AddJoin(string OriginTable, string OriginValue, string ConnectorTable, string ConnectorValue)
		{
			RequestJoin newJoin = new RequestJoin(OriginTable, OriginValue, ConnectorTable, ConnectorValue);
			AddJoin(newJoin);
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
		public void AddWhere(string Table, string ValueName, object Value)
		{
			RequestWhere newWhere = new RequestWhere(Table, ValueName, Value);
			AddWhere(newWhere);
		}


		public string Table { get; set; }
		public List<RequestData> Data { get; set; } = new List<RequestData>();
		public List<RequestJoin> Joins { get; set; } = new List<RequestJoin>();
		public List<RequestWhere> WhereData { get; set; } = new List<RequestWhere>();
	}
}
