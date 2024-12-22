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
				string connString = "server=192.168.1.193;uid=TaskBoxAdmin;pwd=TaskBoxPassword;database=TaskBox";
				_connection = new MySqlConnection(connString);

				_connection.Open();

				return true;
			}
			catch
			{
				Console.WriteLine("Failed to connect to db");
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
			Console.WriteLine("Connecting");
			if (ConnectToDatabase() == false)
				return new List<T>();

			string query = "SELECT ";

			//Data to be selected
			string dataQuery = "";
			foreach (RequestData requestData in request.Data)
			{
				dataQuery += $"{requestData.Table}.{requestData.ValueName} as {requestData.ParseTo}, ";
			}

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
				}

				query = query.Substring(0, query.Length - 5);
			}
			query += ";";

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
	}
}
