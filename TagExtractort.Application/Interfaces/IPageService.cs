using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagExtractort.Application.Dtos;

namespace TagExtractort.Application.Interfaces
{
    public interface IPageService
    {
        Task EqueueExtractKeywords(ExtractKeywordsRequestDto dto);
    }
}