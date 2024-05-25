using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagExtractort.Domain.Entities;

namespace TagExtractort.Application.Interfaces
{
    public interface IElasticSearchService
    {
        Task AddTagAsync(Tag tag);
        public Task<string> SearchForSite(string tagName);
    }
}