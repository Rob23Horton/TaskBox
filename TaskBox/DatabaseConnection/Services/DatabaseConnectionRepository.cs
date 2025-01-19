using DatabaseConnection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Security.Cryptography;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using DatabaseConnection.Attributes;

namespace DatabaseConnection.Services
{
	public class DatabaseConnectionRepository : IDatabaseConnection
	{
		private MySqlConnection _connection;

		private bool ConnectedToDatabase()
		{
			try
			{
				_connection.Ping();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool ConnectToDatabase()
		{
			try
			{
				string connString = "server=localhost;uid=TaskBoxAdmin;pwd=TaskBoxPassword;database=TaskBox";
				_connection = new MySqlConnection(connString);

				_connection.Open();

				return true;
			}
			catch
			{
				Console.WriteLine($"Failed to connect to database at {DateTime.Now}");
				return false;
			}
		}

		private void DisconnectFromDatabase()
		{
			try
			{
				if (_connection is null)
				{
					return;
				}

				_connection.Close();
			}
			catch
			{

			}
		}


		public List<T> Select<T>(SelectRequest request)
		{
			if (ConnectToDatabase() == false)
				return new List<T>();

			string query = "SELECT ";

			//Data to be selected
			string dataQuery = "";
			foreach (RequestData requestData in request.Data)
			{
				dataQuery += $"{requestData.Table}.{requestData.ValueName} as {requestData.ParseTo}, ";
			}

			string subQueryQuery = "";
			//Adds Subqueries
			foreach (SubQuery subQuery in request.SubQueries)
			{
				try
				{
					string currentQuery = "";

					RequestData currData = subQuery.Select.Data[0];
					currentQuery += $"(SELECT ";

					switch (subQuery.Function)
					{
						case Functions.Count:
							currentQuery += "COUNT(";
							break;

						case Functions.Sum:
							currentQuery += "SUM(";
							break;

						case Functions.Average:
							currentQuery += "AVG(";
							break;

						case Functions.TimeDiff:

							currentQuery += $"TIMEDIFF({currData.Table}.{currData.ValueName}, ";
							currData = subQuery.Select.Data[1];

							break;
					}

					currentQuery += $"{currData.Table}.{currData.ValueName}) ";
					if (subQuery.Select.Table != request.Table)
					{
						currentQuery += $"FROM { subQuery.Select.Table} ";
					}

					//Joins
					foreach (RequestJoin join in subQuery.Select.Joins)
					{
						currentQuery += $"INNER JOIN {join.ConnectorTable} ON {join.OriginTable}.{join.OriginValue} = {join.ConnectorTable}.{join.ConnectorValue} ";
					}

					currentQuery += "WHERE ";

					foreach (RequestWhere where in subQuery.Select.WhereData)
					{
						currentQuery += $"{where.Table}.{where.ValueName} = ";

						if (where.Value is string strVal)
						{
							currentQuery += $"'{strVal}' AND ";
						}
						else if (where.Value is int intVal)
						{
							currentQuery += $"{intVal} AND ";
						}
						else if (where.Value is bool boolVal)
						{
							currentQuery += $"b'{(boolVal ? '1' : '0')}' AND ";
						}
						else if (where.Value is DateTime dateVal)
						{
							currentQuery += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}' AND ";
						}
						else if (where.Value is AnotherTableValue anthTblVal)
						{
							currentQuery += $"{anthTblVal.Table}.{anthTblVal.ValueName} AND ";
						}
					}
					currentQuery = currentQuery.Substring(0, currentQuery.Length - 5);

					subQueryQuery += $"{currentQuery}) As {subQuery.As}, ";
				}
				catch
				{
					continue;
				}
			}
			dataQuery += subQueryQuery;

			dataQuery = dataQuery.Substring(0, dataQuery.Length - 2);
			query += $"{dataQuery} FROM {request.Table} ";

			//Joins
			foreach (RequestJoin join in request.Joins)
			{
				query += $"INNER JOIN {join.ConnectorTable} ON {join.OriginTable}.{join.OriginValue} = {join.ConnectorTable}.{join.ConnectorValue} ";
			}

			//Where
			if (request.WhereData.Count() > 0)
			{
				query += "WHERE ";
				foreach (RequestWhere where in request.WhereData)
				{
					query += $"{where.Table}.{where.ValueName} = ";

					if (where.Value is string strVal)
					{
						query += $"'{strVal}' AND ";
					}
					else if (where.Value is int intVal)
					{
						query += $"{intVal} AND ";
					}
					else if (where.Value is bool boolVal)
					{
						query += $"b'{(boolVal ? '1' : '0')}' AND ";
					}
					else if (where.Value is DateTime dateVal)
					{
						query += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}' AND ";
					}
					else if (where.Value is AnotherTableValue anthTblVal)
					{
						query += $"{anthTblVal.Table}.{anthTblVal.ValueName} AND ";
					}
				}

				query = query.Substring(0, query.Length - 5);
			}
			query += ";";

			Console.WriteLine($"SELECT - {query}");

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			MySqlDataReader dataReader = cmd.ExecuteReader();

			Type tType = typeof(T);
			PropertyInfo[] propertyInfo = tType.GetProperties();

			List<T> data = new List<T>();

			while (dataReader.Read())
			{
				T item = (T)Activator.CreateInstance(tType);

				foreach (PropertyInfo currentPropertyInfo in propertyInfo)
				{
					try
					{
						if (currentPropertyInfo.PropertyType == typeof(bool))
						{
							currentPropertyInfo.SetValue(item, dataReader.GetBoolean(currentPropertyInfo.Name));
						}
						else
						{
							currentPropertyInfo.SetValue(item, dataReader[currentPropertyInfo.Name]);
						}
					}
					catch
					{
						continue;
					}
				}

				data.Add(item);
			}

			dataReader.Close();
			DisconnectFromDatabase();

			return data;
		}

		public void Insert(InsertRequest request)
		{


			string query = $"INSERT INTO {request.Table} ";

			string valueNames = "(";
			string values = "(";


			foreach (InsertData insertData in request.Data)
			{
				valueNames += $"{insertData.ValueName}, ";

				object value = insertData.Value;

				if (value is string strVal)
				{
					values += $"'{strVal}', ";
				}
				else if (value is int intVal)
				{
					values += $"{intVal}, ";
				}
				else if (value is bool boolVal)
				{
					values += $"b'{(boolVal ? '1' : '0')}', ";
				}
				else if (value is DateTime dateVal)
				{
					values += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}', ";
				}
			}

			valueNames = $"{valueNames.Substring(0, valueNames.Length - 2)})";
			values = $"{values.Substring(0, values.Length - 2)})";

			query += $"{valueNames} VALUES {values};";

			Console.WriteLine(query);

			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void Insert<T>(InsertRequest request, T insertData)
		{
			string query = $"INSERT INTO {request.Table} ";

			string valueNames = "(";
			string values = "(";

			Type tType = typeof(T);
			PropertyInfo[] propertyInfo = tType.GetProperties();

			foreach (PropertyInfo currentPropertyInfo in propertyInfo.Where(p => !Attribute.IsDefined(p, typeof(InsertIgnore))))
			{
				valueNames += $"{currentPropertyInfo.Name}, ";

				object value = currentPropertyInfo.GetValue(insertData)!;

				if (value is string strVal)
				{
					values += $"'{strVal}', ";
				}
				else if (value is int intVal)
				{
					values += $"{intVal}, ";
				}
				else if (value is bool boolVal)
				{
					values += $"b'{(boolVal ? '1' : '0')}', ";
				}
				else if (value is DateTime dateVal)
				{
					values += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}', ";
				}
			}

			valueNames = $"{valueNames.Substring(0, valueNames.Length - 2)})";
			values = $"{values.Substring(0, values.Length - 2)})";

			query += $"{valueNames} VALUES {values};";

			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void Update(UpdateRequest request)
		{
			string query = $"UPDATE {request.Table} SET";

			string queryValues = "";
			foreach (KeyValuePair<string, object> Value in request.Values)
			{
				queryValues += $"{Value.Key} = ";

				if (Value.Value is string strVal)
				{
					queryValues += $"'{strVal}', ";
				}
				else if (Value.Value is int intVal)
				{
					queryValues += $"{intVal}, ";
				}
				else if (Value.Value is bool boolVal)
				{
					queryValues += $"b'{(boolVal ? '1' : '0')}', ";
				}
				else if (Value.Value is DateTime dateVal)
				{
					queryValues += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}', ";
				}
			}

			queryValues = $"{queryValues.Substring(0, queryValues.Length - 2)}";

			query = $"{query} {queryValues} WHERE {request.Table}.{request.WhereData[0].ValueName} = {request.WhereData[0].Value};";

			Console.WriteLine($"Update Query - {query}");

			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void Update<T>(UpdateRequest request, T updateData)
		{
			string query = $"UPDATE {request.Table} SET ";

			Type tType = typeof(T);
			PropertyInfo[] propertyInfo = tType.GetProperties();

			foreach (PropertyInfo currentPropertyInfo in propertyInfo.Where(p => !Attribute.IsDefined(p, typeof(InsertIgnore))))
			{
				query += $"{currentPropertyInfo.Name} = ";

				object value = currentPropertyInfo.GetValue(updateData)!;

				if (value is string strVal)
				{
					query += $"'{strVal}', ";
				}
				else if (value is int intVal)
				{
					query += $"{intVal}, ";
				}
				else if (value is bool boolVal)
				{
					query += $"b'{(boolVal ? '1' : '0')}', ";
				}
				else if (value is DateTime dateVal)
				{
					query += $"'{dateVal.ToString("yyyy-MM-dd HH:mm:ss")}', ";
				}
			}
			query = $"{query.Substring(0, query.Length - 2)}";

			query += $" WHERE {request.Table}.{request.WhereData[0].ValueName} = {request.WhereData[0].Value};";

			Console.WriteLine($"Update Query - {query}");

			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}

		public void Delete(DeleteRequest request)
		{
			string query = $"DELETE FROM {request.Table} WHERE ({request.ColumnName} = {request.Id});";

			if (ConnectToDatabase() == false)
				return;

			MySqlCommand cmd = new MySqlCommand(query, _connection);
			cmd.ExecuteNonQuery();

			DisconnectFromDatabase();
		}
	}
}
