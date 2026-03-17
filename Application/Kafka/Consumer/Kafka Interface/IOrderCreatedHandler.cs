using System;
using System.Collections.Generic;
using System.Text;
using Application.Kafka.Consumer.Kafka_Entities;

namespace Application.Kafka.Consumer.Kafka_Interface
{
	public interface IOrderCreatedHandler
	{
		Task HandleAsync(OrderCreatedEvent evt, CancellationToken ct);
	}
}
