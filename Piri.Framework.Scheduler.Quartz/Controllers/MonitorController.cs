using Microsoft.AspNetCore.Mvc;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IScheduleJob _scheduleJob;
        public MonitorController(IJobService jobService, IScheduleJob scheduleJob)
        {
            _jobService = jobService;
            _scheduleJob = scheduleJob;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> List()
        {
            Result<List<QuartzDto>> result = await _scheduleJob.GetAllWorkingJobs();
            List<Result<JobDto>> jobResult = new List<Result<JobDto>>();
            foreach (var item in result.Data)
            {
                jobResult.Add(await _jobService.GetJobByName(item.JobKeyName));

            }

            return View(jobResult);
        }
    }
}
