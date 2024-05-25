using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using TagExtractort.Application.Interfaces;

namespace TagExtractort.Application.Services
{
    public class KafkaProduceService : IKafkaProduceService
    {
        private readonly IProducer<Null, string> producer;
        public KafkaProduceService()
        {
            var pConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

            producer = new ProducerBuilder<Null, string>(pConfig).Build();
        }

        ~KafkaProduceService()
        {
            producer.Dispose();
        }
        public async Task Produce(string message)
        {
            await producer.ProduceAsync("testtopic", new Message<Null, string> { Value = message });
        }
    }
}