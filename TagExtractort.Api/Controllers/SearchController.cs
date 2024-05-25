using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TagExtractort.Application.Interfaces;

namespace TagExtractort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        readonly IElasticSearchService _elasticSearchService;
        public SearchController(IElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchByTagForSite(string tagName)
        {
            var response = await _elasticSearchService.SearchForSite(tagName);
            return Ok(response);
        }
    }
}