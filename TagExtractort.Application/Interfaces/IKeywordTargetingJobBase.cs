using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagExtractort.Application.Interfaces
{
    public interface IKeywordTargetingJobBase : IJob
    {
        void CancelExecution();
    }
}