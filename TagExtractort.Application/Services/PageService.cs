using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagExtractort.Application.Dtos;
using TagExtractort.Application.Interfaces;

namespace TagExtractort.Application.Services
{
    public class PageService : IPageService
    {
        private IKafkaProduceService _kafkaProduceService;
        public PageService(IKafkaProduceService kafkaService)
        {
            _kafkaProduceService = kafkaService;
        }

        //TODO: check url validity
        public async Task EqueueExtractKeywords(ExtractKeywordsRequestDto dto)
        {
            await _kafkaProduceService.Produce(dto.Url);
        }
    }
}