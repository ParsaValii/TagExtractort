using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using TagExtractort.Application.Interfaces;

namespace TagExtractort.Application.Services
{
    public class KafkaConsumeService : IKafkaConsumeService
    {
        private readonly IConsumer<Ignore, string> consumer;
        private CancellationTokenSource _consumerCancellation = null;
        public KafkaConsumeService()
        {
            var cConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "GroupOne",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumerCancellation = new CancellationTokenSource();

            consumer = new ConsumerBuilder<Ignore, string>(cConfig).Build();
        }

        ~KafkaConsumeService()
        {
            consumer.Dispose();
        }

        public void Subscribe(string topic = null)
        {
            consumer.Subscribe(topic);
        }

        public ConsumeResult<Ignore, string> Consume()
        {
            return consumer.Consume(_consumerCancellation.Token);
        }

        public void Commit(ConsumeResult<Ignore, string> consumeResult)
        {
            consumer.Commit(consumeResult);
        }

        public void Consume(Func<string, Null> func, string topic = null)
        {
            _consumerCancellation = new CancellationTokenSource();

            Subscribe(topic);

            while (true)
            {
                var consumeResult = Consume();
                func(consumeResult.Message.Value);
                Commit(consumeResult);
            }
        }
    }
}