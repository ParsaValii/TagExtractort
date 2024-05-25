using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TagExtractort.Application.Dtos;
using TagExtractort.Application.Interfaces;

namespace TagExtractort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageController : ControllerBase
    {
        readonly IPageService _pageService;
        readonly IElasticSearchService _elasticSearchService;
        public PageController(IPageService pageService, IElasticSearchService elasticSearchService)
        {
            _pageService = pageService;
            _elasticSearchService = elasticSearchService;
        }
        [HttpPost]
        public async Task<IActionResult> EqueueExtractKeywords([FromBody] ExtractKeywordsRequestDto dto)
        {
            await _pageService.EqueueExtractKeywords(dto);
            return Ok();
        }
    }
}