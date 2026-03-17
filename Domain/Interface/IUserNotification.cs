using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Interface
{
	public interface IUserNotification
	{
	 Task<EUserNotification> SendNotificationAsync(EUserNotification request);
	}
}
