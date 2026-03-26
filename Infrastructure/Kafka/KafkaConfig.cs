using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Kafka
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
