using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Kafka
{
    public class KafkaProducer
    {
        private readonly KafkaConfig _config;

        public KafkaProducer(KafkaConfig config)
        {
            _config = config;
        }

        public async Task ProduceAsync(string topic, string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _config?.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = _config?.ApiKey,
                SaslPassword = _config?.ApiSecret
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }
}

