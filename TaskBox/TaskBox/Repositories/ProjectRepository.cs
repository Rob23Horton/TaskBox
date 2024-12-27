﻿using DatabaseConnection.Models;
using DatabaseConnection.Services;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly IDatabaseConnection _databaseConnection;
		public ProjectRepository(IDatabaseConnection databaseConnection)
		{
			this._databaseConnection = databaseConnection;
		}

		public bool CheckProjectPermission(int UserId, int ProjectId)
		{
			try
			{
				SelectRequest permissionRequest = new SelectRequest("tblProjectUser");
				permissionRequest.AddData("ProjectUserId");

				permissionRequest.AddWhere("UserCode", UserId);
				permissionRequest.AddWhere("ProjectCode", ProjectId);

				List<ProjectUserPermission> permission = _databaseConnection.Select<ProjectUserPermission>(permissionRequest);

				if (permission.Count() == 0)
				{
					return false;
				}

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
