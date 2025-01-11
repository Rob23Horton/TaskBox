using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Models
{
	public class SubQuery
	{
		public SubQuery()
		{
		}

		public SubQuery(Functions Function, SelectRequest Select, string As)
		{
			this.Function = Function;
			this.Select = Select;
			this.As = As;
		}


		public Functions Function { get; set; }
		public SelectRequest Select { get; set; }
		public string As { get; set; }
	}
}
