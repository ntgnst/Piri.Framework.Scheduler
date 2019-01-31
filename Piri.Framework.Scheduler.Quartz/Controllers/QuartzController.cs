using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    [Route("api/quartz")]
    public class QuartzController : ControllerBase
    {
        IJobService _jobService;
        private readonly ILogger _logger;
        private readonly IScheduleJob _scheduleJob;
        public QuartzController(IJobService jobService, IScheduleJob scheduleJob ,ILogger<QuartzController> logger)
        {
            _logger = logger;
            _jobService = jobService;
            _scheduleJob = scheduleJob;
        }

        /// <summary>
        /// Creates a scheduled Job.
        /// </summary>
        /// <param name="jobDataDto"></param>
        /// <returns>QuartzDto</returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - A Scheduled Job Created</remarks>
        [HttpPost]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<Result<QuartzDto>> Post([FromBody]JobDataDto jobDataDto)
        {
            Guid guid = Guid.NewGuid();
            string jobName = $"{typeof(SimpleTestProcess).FullName}-{guid}";
            jobDataDto.Name = jobName;
            JobDto jobDto = new JobDto()
            {
                Guid = guid,
                IsActive = true,
                IsRunning = false,
                JobDataDtoList = new List<JobDataDto>()
                {
                    jobDataDto
                },
                LastEndTime = null,
                LastRunTime = null,
            };
            Result<QuartzDto> result = await _scheduleJob.AddJob<SimpleTestProcess>(jobDto, "", true);
            if (result.IsSuccess)
            {
                jobDto.IsRunning = true;
                await _jobService.AddJob(jobDto);
            }

            return result;
        }
        /// <summary>
        /// Gets list of scheduled Jobs.
        /// </summary>
        /// <param></param>
        /// <returns>
        /// <list type="JobDto"></list>
        /// </returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - Scheduled Jobs Listed</remarks>
        [HttpGet]
        public async Task<List<Result<JobDto>>> Get()
        {
            Result<List<QuartzDto>> result = await _scheduleJob.GetAllWorkingJobs();
            List<Result<JobDto>> jobResult = new List<Result<JobDto>>();
            foreach (var item in result.Data)
            {
                jobResult.Add(await _jobService.GetJobByName(item.JobKeyName));

            }
            _logger.LogInformation("api/quartz/get replied :",jobResult);
            return jobResult;
        }
        /// <summary>
        /// Pauses all scheduled Jobs.
        /// </summary>
        /// <param></param>
        /// <returns>string</returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - All scheduled Jobs Paused </remarks>
        [HttpGet("PauseAll")]
        public async Task<Result<string>> PauseAll()
        {
            Result<string> result = await _scheduleJob.PauseAllJobs();
            //TODO : When pause all jobs , update job props.
            return result;
        }
        /// <summary>
        /// Resumes all scheduled Jobs.
        /// </summary>
        /// <param></param>
        /// <returns>string</returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - All scheduled Jobs Resumed </remarks>
        [HttpGet("ResumeAll")]
        public async Task<Result<string>> ResumeAll()
        {
            Result<string> result = await _scheduleJob.ResumeAllJobs();
            //TODO : When resume all jobs , update job props.
            return result;
        }
        /// <summary>
        /// Initializes all scheduled Jobs.
        /// </summary>
        /// <param></param>
        /// <returns>string</returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - All scheduled Jobs Initialized </remarks>
        [HttpGet("InitializeAllJobs")]
        public async Task<Result<List<Result<JobDto>>>> InitializeAllJobs()
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
            return result;
        }
        /// <summary>
        /// Initializes a scheduled Job.
        /// </summary>
        /// <param></param>
        /// <returns>string</returns>
        /// <remarks>
        /// ### REMARKS ###
        /// The following codes are returned
        /// - 400 - No sub domain is found that matches the SubDomainName property
        /// - 200 - The scheduled Job Initialized </remarks>
        [HttpGet("InitializeJob")]
        public async Task<JsonResult> InitializeJob()
        {
            return null;
        }
    }
}