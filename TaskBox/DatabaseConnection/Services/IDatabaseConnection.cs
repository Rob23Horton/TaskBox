﻿using DatabaseConnection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Services
{
	public interface IDatabaseConnection
	{
		public List<T> Select<T>(SelectRequest request);
		public void Insert(InsertRequest request);
		public void Insert<T>(InsertRequest request, T insertData);
	}
}
