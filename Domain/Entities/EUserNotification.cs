using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
	public class EUserNotification
	{  

	    [Key]
	   public int Id { get; set; }
		public int UserId { get; set; }
		public string Message { get; set; }
		public DateTime CreatedDate { get; set; }


	}
}
