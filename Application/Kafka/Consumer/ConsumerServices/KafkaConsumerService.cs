using System;
using System.Collections.Generic;
using System.Text;
using Application.Kafka.Consumer.Kafka_Entities;
using Application.Kafka.Consumer.Kafka_Interface;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Kafka.Consumer.ConsumerServices
{
	public class KafkaConsumerService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IConfiguration config;
		private readonly ILogger<KafkaConsumerService> _logger;

		public KafkaConsumerService(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<KafkaConsumerService> logger)
		{
			_scopeFactory = scopeFactory;
			config = configuration;
			_logger = logger;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumerConfig = new ConsumerConfig
			{
				BootstrapServers = config["Kafka:BootstrapServers"],
				GroupId = config["Kafka:ConsumerGroupId"],
				AutoOffsetReset = AutoOffsetReset.Earliest,
				EnableAutoCommit = false

			};
			using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

			var topics = config.GetSection("Kafka:Topics").Get<List<string>>()!;
			consumer.Subscribe(topics);
			_logger.LogInformation("Kafka consumer started, listening on: {Topics}", string.Join(", ", topics));

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var result = consumer.Consume(stoppingToken);

					_logger.LogInformation("Received Message from topic {Topic}: {Message}", result.Topic, result.Message.Value);
					await HandleMessageAsync(result.Topic, result.Message.Value,stoppingToken);

					consumer.Commit(result);
				}
				catch (OperationCanceledException)
				{
					_logger.LogInformation("Kafka consumer is stopping.");
					break;

				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error consuming message from Kafka.");
					await Task.Delay(1000, stoppingToken); // Wait before retrying
				}

			}
			consumer.Close();


		}
		private async Task HandleMessageAsync(string topic, string value, CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();

			switch (topic)
			{
				case KafkaTopics.OrderCreated:
					var orderCreated = JsonConvert.DeserializeObject<OrderCreatedEvent>(value)!;
					var orderHandler = scope.ServiceProvider.GetRequiredService<IOrderCreatedHandler>();
					await orderHandler.HandleAsync(orderCreated, ct);
					break;


				default:
					_logger.LogWarning("No handler for topic: {Topic}", topic);
					break;
			}
		}
	}
}