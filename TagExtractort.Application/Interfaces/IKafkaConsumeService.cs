using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace TagExtractort.Application.Interfaces
{
    public interface IKafkaConsumeService
    {
        void Subscribe(string topic = null);
        ConsumeResult<Ignore, string> Consume();
        void Commit(ConsumeResult<Ignore, string> consumeResult);
    }
}