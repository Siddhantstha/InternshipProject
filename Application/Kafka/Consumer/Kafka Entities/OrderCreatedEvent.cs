using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Kafka.Consumer.Kafka_Entities
{
	public class OrderCreatedEvent
	{
	    public int UserId { get; set; }	
		public string Message { get; set; }
	}
}
