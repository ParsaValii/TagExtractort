using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json.Nodes;
using TagExtractort.Application.Interfaces;
using HtmlAgilityPack;
using TagExtractort.Domain.Entities;

namespace TagExtractort.Application.Services
{
    public class KeywordTargetingJobBase : IKeywordTargetingJobBase
    {
        private readonly IKafkaConsumeService _kafkaConsumer;
        readonly object _locker;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public KeywordTargetingJobBase(IKafkaConsumeService kafkaConsumeService, IElasticSearchService elasticSearchService, CancellationTokenSource cancellationTokenSource)
        {
            this._elasticSearchService = elasticSearchService;
            this._kafkaConsumer = kafkaConsumeService;
            _locker = new();
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void CancelExecution()
        {
            _cancellationTokenSource.Cancel();
        }

        public async Task Execute()
        {
            _kafkaConsumer.Subscribe("testtopic");

            var tasks = new Task[5];

            for (int i = 0; i < 5; i++)
                tasks[i] = Task.Run(() => ConsumeMessage());

            await Task.WhenAll(tasks);
        }

        private async Task ConsumeMessage()
        {
            var consumeResult = KafkaConsume();
            await ProcessMessage(consumeResult);
        }

        private ConsumeResult<Ignore, string> KafkaConsume()
        {
            lock (_locker)
            {
                return _kafkaConsumer.Consume();
            }
        }
        private async Task ProcessMessage(ConsumeResult<Ignore, string> consumeResult)
        {
            var web = new HtmlWeb();

            var url = consumeResult.Message.Value;
            // string url = (string)jsonNode["Url"];
            var doc = web.Load(url);
            var keywordsMetaTag = doc.DocumentNode
                .Descendants("meta")
                .FirstOrDefault(meta =>
                    meta.Attributes["name"] != null &&
                    meta.Attributes["name"].Value.ToLower() == "keywords");

            string keywordsContent = keywordsMetaTag.Attributes["content"].Value;

            string[] keywordsArray = keywordsContent.Split(',');

            foreach (string keyword in keywordsArray)
            {
                var tag = new Tag { Name = keyword.Trim(), Url = url };
                await _elasticSearchService.AddTagAsync(tag);
            }
        }
    }
}