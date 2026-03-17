using System;
using System.Collections.Generic;
using System.Text;
using Application.Kafka.Consumer.Kafka_Entities;
using Application.Kafka.Consumer.Kafka_Interface;
using Confluent.Kafka;
using Domain.Entities;
using Domain.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Kafka.Consumer.ConsumerServices
{
	public class OrderCreatedHandler : IOrderCreatedHandler
	{
		private readonly ILogger<OrderCreatedHandler> _logger;
		private readonly IUserNotification _userNotification;

		public OrderCreatedHandler(ILogger<OrderCreatedHandler> logger, IUserNotification userRepository)
		{
			_logger = logger;
			_userNotification = userRepository;



		}
		public async Task HandleAsync(OrderCreatedEvent evt, CancellationToken ct)

		{
			_logger.LogInformation("Processing OrderCreated for User {UserId}", evt.UserId);

			var notification = new EUserNotification
			{
				UserId = evt.UserId,
				Message = evt.Message,
				CreatedDate = DateTime.Now
			};
			// ✅ Call your IUserNotification interface
			await _userNotification.SendNotificationAsync(notification);

		}
	}
}