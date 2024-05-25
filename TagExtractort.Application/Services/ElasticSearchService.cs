using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using TagExtractort.Application.Interfaces;
using TagExtractort.Domain.Entities;

namespace TagExtractort.Application.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _client;

        public ElasticSearchService()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("testindex");

            _client = new ElasticClient(settings);

        }
        public async Task AddTagAsync(Tag tag)
        {
            var response = await _client.IndexDocumentAsync(tag);
        }

        public async Task<string> SearchForSite(string tagName)
        {
            var searchResponse = await _client.SearchAsync<Tag>(s => s
                .Query(q => q
                    .Match(m => m
                    .Field(f => f.Name == tagName)))
                    .Size(1000));
            var response = searchResponse.Documents.First(x => x.Name == tagName);
            return response.Url;
        }
    }
}