using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Kafka.Consumer.Kafka_Entities
{
	public enum OrderStatus
	{
		Approved = 1,
		Rejected = 2,
		Pending = 3,
		Completed = 4,
		Canceled = 5,


	}
}
