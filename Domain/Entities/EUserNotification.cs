using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
	public class EUserNotification
	{
		public int UserId { get; set; }
		public string Message { get; set; }
		public DateTime CreatedDate { get; set; }


	}
}
