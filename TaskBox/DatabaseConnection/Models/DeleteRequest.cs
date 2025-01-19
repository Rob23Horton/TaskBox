using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class DeleteRequest
	{
		public DeleteRequest()
		{
			
		}

		public DeleteRequest(string Table, string PrimaryKeyColumnName, int Id)
		{
			this.Table = Table;
			this.ColumnName = PrimaryKeyColumnName;
			this.Id = Id;
		}

		public string Table { get; set; }
		public string ColumnName { get; set; }
		public int Id { get; set; }
	}
}
