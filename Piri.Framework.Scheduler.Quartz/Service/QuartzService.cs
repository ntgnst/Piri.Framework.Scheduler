﻿using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Service
{
    public class QuartzService : IScheduleJob
    {
        private readonly IJobService _jobService;
        private IScheduler _scheduler;
        private ICronTrigger _cronTrigger;
        private IJobDetail _job;
        private TriggerBuilder _triggerBuilder;
        private ITrigger _trigger;
        private string _jobName;
        public QuartzService(IJobService jobService)
        {
            _jobService = jobService;
        }
        //Creates and starts a job
        public async Task<Result<QuartzDto>> StartJob<TJob>(JobDto jobDto, bool isStartNow = false) where TJob : IJob
        {
            Result<QuartzDto> result;
            try
            {
                _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                await _scheduler.Start();

                _jobName = jobDto.JobDataDtoList.FirstOrDefault().Name;

                _job = JobBuilder.Create<TJob>()
                    .WithIdentity($"{_jobName}.{jobDto.Guid}")
                    .Build();

                //_cronTrigger = FireOrAddTrigger();
                if (isStartNow)
                {
                    _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity($"{_jobName}.{jobDto.Guid}.trigger")
                    .StartNow()
                    .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
                    .Build();
                }
                else
                {
                    _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity($"{_jobName}.{jobDto.Guid}.trigger")
                    .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
                    .Build();
                }

                await _scheduler.ScheduleJob(_job, _cronTrigger);
                result = new Result<QuartzDto>(new QuartzDto() { Name = _jobName, Description = _job?.Description, JobKeyName = _job.Key.ToString(), IsActive = true, IsRunning = true });
            }
            catch (Exception ex)
            {
                result = new Result<QuartzDto>(ResultTypeEnum.Error, null, $"An error occured while creating and starting job. Ex : {ex.ToString()}");
            }
            return result;
        }
        public async Task<Result<JobDto>> TriggerJob<TJob>(JobDto jobDto) where TJob : IJob
        {
            Result<JobDto> result;
            try
            {
                _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await _scheduler.Start();
                _jobName = $"{typeof(TJob).FullName}-{jobDto.Guid}";
                _job = JobBuilder.Create<TJob>()
                    .WithIdentity($"{_jobName}.{jobDto.Guid}")
                    .StoreDurably(true)
                    .Build();

                _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity($"{_jobName}.{jobDto.Guid}.trigger")
                    .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
                    .Build();


                _triggerBuilder = TriggerBuilder.Create();

                _trigger = _triggerBuilder
                                .ForJob(_job.Key)
                                .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
                                .WithIdentity($"{_jobName}.{jobDto.Guid}")
                                .Build();

                await _scheduler.ScheduleJob(_job, _cronTrigger);
                result = new Result<JobDto>(jobDto);
            }
            catch (Exception ex)
            {
                result = new Result<JobDto>(false, $"An error occured while triggering job. Ex : {ex.ToString()}");
            }
            return result;
        }
        //Adds new job Without starting
        public async Task<Result<QuartzDto>> AddJob<TJob>(JobDto jobDto, string description = "") where TJob : IJob
        {
            Result<QuartzDto> result;
            try
            {
                if (CronExpression.IsValidExpression(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex))
                {
                    _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                    await _scheduler.Start();
                    _jobName = jobDto.JobDataDtoList.FirstOrDefault().Name;
                    _job = CreateJobDetail<TJob>(description);

                    if (await _scheduler.CheckExists(_job.Key))
                    {
                        result = new Result<QuartzDto>(false, ResultTypeEnum.Error, "Given JobKey is already exist.");
                    }
                    else
                    {
                        _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                                        .WithIdentity($"{_jobName}.{jobDto.Guid}.trigger")
                                        .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
                                        .Build();

                        await _scheduler.AddJob(_job, false, true);
                        result = new Result<QuartzDto>(new QuartzDto() { Name = _job?.Key.ToString(), Description = _job?.Description, JobKeyName = _job.Key.ToString() });
                    }
                }
                else
                {
                    result = new Result<QuartzDto>(false, "Cron Regex is invalid.");
                }
            }
            catch (Exception ex)
            {
                result = new Result<QuartzDto>(false, ex.ToString());
            }

            return result;
        }
        public async Task<Result<List<QuartzDto>>> GetAllWorkingJobs()
        {
            Result<List<QuartzDto>> result;
            List<QuartzDto> modelList = new List<QuartzDto>();
            try
            {
                _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                IReadOnlyCollection<string> jobGroupList = await _scheduler.GetJobGroupNames();
                foreach (string jobGroup in jobGroupList)
                {
                    GroupMatcher<JobKey> groupMatcher = GroupMatcher<JobKey>.GroupContains(jobGroup);
                    IReadOnlyCollection<JobKey> jobKeys = await _scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        IJobDetail detail = await _scheduler.GetJobDetail(jobKey);
                        IReadOnlyCollection<ITrigger> triggers = await _scheduler.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {

                            modelList.Add(new QuartzDto()
                            {
                                Group = jobGroup,
                                JobKeyName = jobKey.Name,
                                Description = detail.Description,
                                Name = trigger.Key.Name,
                                NextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.LocalDateTime.ToString() : "No Information",
                                PreviousFireTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetPreviousFireTimeUtc().Value.ToString() : "No Information"
                            });
                        }
                    }
                }
                result = new Result<List<QuartzDto>>(modelList);
            }
            catch (Exception ex)
            {
                result = new Result<List<QuartzDto>>(false, $"QuartzUtilizationService.GetAllWorkingJobs Method Ex : {ex.ToString()}");
            }

            return result;
        }
        private IJobDetail CreateJobDetail<TJob>(string description = "") where TJob : IJob
        {
            if (description.Equals(""))
            {
                _job = JobBuilder.Create<TJob>()
                                .WithIdentity(_jobName)
                                .Build();
            }
            else
            {
                _job = JobBuilder.Create<TJob>()
                                .WithIdentity(_jobName)
                                .WithDescription(description)
                                .Build();
            }

            return _job;
        }
        public async Task<Result<string>> PauseAllJobs()
        {
            Result<string> result;

            try
            {
                _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await _scheduler.PauseAll();
                result = new Result<string>(true, "All jobs successfully paused.");
            }
            catch (Exception ex)
            {
                result = new Result<string>(true, $"Pausing all jobs failed. Ex : {ex.ToString()}");
            }

            return result;
        }
        public async Task<Result<string>> ResumeAllJobs()
        {
            Result<string> result;

            try
            {
                _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                await _scheduler.ResumeAll();
                result = new Result<string>(true, "All jobs successfully resumed.");
            }
            catch (Exception ex)
            {
                result = new Result<string>(true, $"Resuming all jobs failed. Ex : {ex.ToString()}");
            }

            return result;
        }
        public async Task<Result<List<Result<JobDto>>>> InitializeAllJobs()
        {
            Result<List<Result<JobDto>>> result;
            List<Result<JobDto>> innerResultList = new List<Result<JobDto>>();
            try
            {
                IJobService jobService = new JobService();
                List<JobDto> jobList = jobService.GetAllJobs().Data.Where(w => w.IsActive).ToList();
                if (jobList.Any())
                {
                    _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
                    await _scheduler.Start();
                    foreach (JobDto job in jobList)
                    {
                        innerResultList.Add(await TriggerJob<SimpleTestProcess>(job));
                    }
                    result = new Result<List<Result<JobDto>>>(innerResultList);
                }
                else
                {
                    result = new Result<List<Result<JobDto>>>(false, ResultTypeEnum.Warning, "Initializing jobs failed. There is no active jobs.");
                }
            }
            catch (Exception ex)
            {
                result = new Result<List<Result<JobDto>>>(false, $"An error occured while initializing jobs. Ex : {ex.ToString()}");
            }
            return result;
        }
    }
}