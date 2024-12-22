using DatabaseConnection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Security.Cryptography;

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

			Console.WriteLine("HI");


			throw new NotImplementedException();
		}
	}
}
