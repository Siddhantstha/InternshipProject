using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.DBconnect;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class NotificationRepository : IUserNotification
	{
		private readonly AppDBconnect _dbconnect;
		public NotificationRepository(AppDBconnect dbconnect)
		{
			_dbconnect = dbconnect;

        }
	public async Task <EUserNotification> SendNotificationAsync(EUserNotification request)
		{

			var result = await _dbconnect.EUsers.AddAsync(request);
			await _dbconnect.SaveChangesAsync();

			return result.Entity;

		}
	}
}