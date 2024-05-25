using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using TagExtractort.Application.JobSchedule;

namespace TagExtractort.Application.Interfaces
{
    public interface IJob
    {
        [AutomaticRetry(Attempts = 10)]
        [SkipWhenPreviousJobIsRunning]
        Task Execute();
    }
}