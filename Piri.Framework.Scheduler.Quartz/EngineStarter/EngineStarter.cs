using Microsoft.Extensions.Logging;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.EngineStarter
{
    public class EngineStarter : PiriJob
    {
        private readonly IJobService _jobService;
        private readonly IScheduleJob _scheduler;
        private readonly ILogger _logger;
        /// <summary>
        /// Checks Engine status for unexpected faults. Starts jobs if they are unexpectedly stopped.
        /// </summary>
        /// <param name="jobService"></param>
        /// <param name="scheduler"></param>
        /// <param name="logger"></param>
        public EngineStarter(IJobService jobService, IScheduleJob scheduler,ILogger<EngineStarter> logger)
        {
            _jobService = jobService;
            _scheduler = scheduler;
            _logger = logger;
        }
        public override async Task StartAsync(IJobExecutionContext context)
        {
            Result<List<JobDto>> allJobs = await _jobService.GetAllJobs();
            
            if (allJobs.IsSuccess && allJobs.Data != null)
            {
                Result<List<JobDto>> workingJobs = await _scheduler.GetAllWorkingJobs();
                if (workingJobs.IsSuccess && workingJobs.Data != null)
                {
                    allJobs.Data.Where(s => s.Guid.Equals(workingJobs.Data.FirstOrDefault(w => w.JobDataDtoList.FirstOrDefault().Name == s.JobDataDtoList.FirstOrDefault().Name)));
                }
            }
        }
    }
}
