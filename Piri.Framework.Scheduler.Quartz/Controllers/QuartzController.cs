using Microsoft.AspNetCore.Mvc;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    [Route("api/quartz")]
    public class QuartzController : Controller
    {
        IJobService _jobService;

        private readonly IScheduleJob _scheduleJob;
        public QuartzController(IJobService jobService, IScheduleJob scheduleJob)
        {
            _jobService = jobService;
            _scheduleJob = scheduleJob;
        }
        [Route("AddJob")]
        public async Task<JsonResult> AddJob()
        {
            Guid guid = Guid.NewGuid();
            string jobName = $"{typeof(SimpleTestProcess).FullName}-{guid}";
            JobDto jobDto = new JobDto()
            {
                Guid = guid,
                IsActive = true,
                IsRunning = false,
                JobDataDtoList = new List<JobDataDto>()
                {
                    new JobDataDto()
                    {
                        Header = "testo",
                        IsRetry = true,
                        Body = "awdawd",
                        Method = "POST",
                        Name = jobName,
                        RetryCount = 10,
                        RetryInterval = 2,
                        TimerRegex = "0/5 * * * * ?",
                        Url = "https://www.google.com.tr"
                    }
                },
                LastEndTime = null,
                LastRunTime = null,
            };
            Result<QuartzDto> result = await _scheduleJob.AddJob<SimpleTestProcess>(jobDto, "A simple job");
            if (result.IsSuccess)
            {
                _jobService.AddJob(jobDto);
            }

            return Json(result);
        }
        [Route("GetList")]
        public async Task<JsonResult> GetList()
        {
            Result<List<QuartzDto>> result = await _scheduleJob.GetAllWorkingJobs();
            return Json(result);
        }

        [HttpGet]
        [Route("PauseAll")]
        public async Task<JsonResult> PauseAll()
        {
            Result<string> result = await _scheduleJob.PauseAllJobs();
            return Json(result);
        }

        [HttpGet]
        [Route("ResumeAll")]
        public async Task<JsonResult> ResumeAll()
        {
            Result<string> result = await _scheduleJob.ResumeAllJobs();
            return Json(result);
        }

        [HttpGet]
        [Route("InitializeAllJobs")]
        public async Task<JsonResult> InitializeAllJobs()
        {
            Result<List<Result<JobDto>>> result = await _scheduleJob.InitializeAllJobs();
            if (result.IsSuccess && result.Data.Any())
            {
                result.Data.ForEach(f =>
                {
                    if (f.IsSuccess)
                    {
                        f.Data.IsRunning = true;
                        _jobService.UpdateJob(f.Data);
                    }
                });
            }
            return Json(result);
        }
    }
}