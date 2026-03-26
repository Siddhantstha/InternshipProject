using Application.Interface;
using Infrastructure.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KafkaMessageProducer : IMessageProducer
    {
        private readonly KafkaProducer _producer;
        public KafkaMessageProducer(KafkaProducer producer)
        {
            _producer = producer;
        }
        public async Task SendMessageAsync(string topic, string message)
        {
            await _producer.ProduceAsync(topic, message);
        }
    }
}
