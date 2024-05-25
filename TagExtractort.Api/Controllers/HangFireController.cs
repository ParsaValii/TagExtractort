using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using TagExtractort.Application.Services;

namespace TagExtractort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangFireController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJob;

        public HangFireController(IBackgroundJobClient backgroundJob)
        {
            _backgroundJob = backgroundJob;
        }

        [HttpPost]
        public IActionResult ScheduleHelloJob()
        {
            RecurringJob.AddOrUpdate<KeywordTargetingJobBase>("ExecuteJob", x => x.Execute(), "* * * * *");

            return Ok("Execute job scheduled!");
        }
    }
}