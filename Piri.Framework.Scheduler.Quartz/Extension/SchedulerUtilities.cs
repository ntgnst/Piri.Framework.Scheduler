using Microsoft.AspNetCore.Builder;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Piri.Framework.Scheduler.Quartz.Service;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public class SchedulerUtilities
    {
        //private static IScheduler _scheduler;
        //private static ICronTrigger _cronTrigger;
        //private static IJobDetail _job;
        //private static TriggerBuilder _triggerBuilder;
        //private static ITrigger _trigger;
        //private static string _jobName;

        ////Creates and starts a job
        //public static async Task<Result<QuartzDto>> StartJob<TJob>(string timerRegex, bool isStartNow = false)
        //       where TJob : IJob
        //{
        //    Result<QuartzDto> result;
        //    try
        //    {
        //        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        //        await _scheduler.Start();

        //        _jobName = typeof(TJob).FullName;

        //        _job = JobBuilder.Create<TJob>()
        //            .WithIdentity(_jobName)
        //            .Build();

        //        //_cronTrigger = FireOrAddTrigger();
        //        if (isStartNow)
        //        {
        //            _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
        //            .WithIdentity($"{_jobName}.trigger")
        //            .StartNow()
        //            .WithCronSchedule(timerRegex)
        //            .Build();
        //        }
        //        else
        //        {
        //            _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
        //            .WithIdentity($"{_jobName}.trigger")
        //            .WithCronSchedule(timerRegex)
        //            .Build();
        //        }

        //        await _scheduler.ScheduleJob(_job, _cronTrigger);
        //        result = new Result<QuartzDto>(new QuartzDto() { Name = _job?.Key.ToString(), Description = _job?.Description, JobKeyName = _job.Key.ToString(), IsActive = true, IsRunning = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<QuartzDto>(ResultTypeEnum.Error, null, $"An error occured while creating and starting job. Ex : {ex.ToString()}");
        //    }
        //    return result;
        //}
        //public static async Task<Result<JobDto>> TriggerJob<TJob>(JobDto jobDto) where TJob : IJob
        //{
        //    Result<JobDto> result;
        //    try
        //    {
        //        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //        await _scheduler.Start();
        //        _jobName = typeof(TJob).FullName;
        //        _job = JobBuilder.Create<TJob>()
        //            .WithIdentity(_jobName)
        //            .StoreDurably(true)
        //            .Build();

        //        _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
        //            .WithIdentity($"{_jobName}.trigger")
        //            .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
        //            .Build();


        //        _triggerBuilder = TriggerBuilder.Create();

        //        _trigger = _triggerBuilder
        //                        .ForJob(_job.Key)
        //                        .WithCronSchedule(jobDto.JobDataDtoList.FirstOrDefault().TimerRegex)
        //                        .WithIdentity(_jobName)
        //                        .Build();

        //        await _scheduler.ScheduleJob(_job, _cronTrigger);
        //        result = new Result<JobDto>(jobDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<JobDto>(false, $"An error occured while triggering job. Ex : {ex.ToString()}");
        //    }
        //    return result;
        //}
        //public static async Task<Result<QuartzDto>> StartAll()
        //{
        //    //TODO : Logic Get All Registered Jobs And Fire Them.
        //    _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //    if (!_scheduler.IsStarted)
        //    {
        //        await _scheduler.Start();
        //    }
        //    return new Result<QuartzDto>();

        //}
        ////Adds new Job Without starting
        //public static async Task<Result<QuartzDto>> AddJob<TJob>(string timerRegex, string description = "") where TJob : IJob
        //{
        //    Result<QuartzDto> result;
        //    try
        //    {
        //        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        //        await _scheduler.Start();
        //        _jobName = typeof(TJob).FullName;
        //        _job = CreateJobDetail<TJob>(description);

        //        if (await _scheduler.CheckExists(_job.Key))
        //        {
        //            result = new Result<QuartzDto>(false, ResultTypeEnum.Error, "Given JobKey is already exist.");
        //        }
        //        else
        //        {
        //            _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
        //                            .WithIdentity($"{_jobName}.trigger")
        //                            .WithCronSchedule(timerRegex)
        //                            .Build();

        //            await _scheduler.AddJob(_job, false, true);
        //            result = new Result<QuartzDto>(new QuartzDto() { Name = _job?.Key.ToString(), Description = _job?.Description, JobKeyName = _job.Key.ToString() });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<QuartzDto>(false, ex.ToString());
        //    }

        //    return result;
        //}
        //public static async Task<Result<List<QuartzDto>>> StartAllJobs()
        //{
        //    Result<List<QuartzDto>> result;
        //    List<QuartzDto> resultList = new List<QuartzDto>();
        //    try
        //    {
        //        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //        await _scheduler.Start();
        //        List<ITrigger> triggerList = new List<ITrigger>();

        //        IReadOnlyCollection<string> jobGroupList = await _scheduler.GetJobGroupNames();
        //        foreach (string jobGroup in jobGroupList)
        //        {
        //            GroupMatcher<JobKey> groupMatcher = GroupMatcher<JobKey>.GroupContains(jobGroup);
        //            IReadOnlyCollection<JobKey> jobkeyList = await _scheduler.GetJobKeys(groupMatcher);

        //            foreach (var jobKey in jobkeyList)
        //            {
        //                IJobDetail detail = await _scheduler.GetJobDetail(jobKey);
        //                IReadOnlyCollection<ITrigger> jobsTriggerList = await _scheduler.GetTriggersOfJob(jobKey);

        //                foreach (ITrigger jobsTrigger in jobsTriggerList)
        //                {
        //                    resultList.Add(new QuartzDto()
        //                    {
        //                        Group = jobGroup,
        //                        JobKeyGroup = jobKey.Group,
        //                        Name = jobKey.Name,
        //                        JobKeyName = jobKey.Name,
        //                        NextFireTime = jobsTrigger.GetNextFireTimeUtc().HasValue ? jobsTrigger.GetNextFireTimeUtc().Value.LocalDateTime.ToString() : "No Information",
        //                        PreviousFireTime = jobsTrigger.GetPreviousFireTimeUtc().HasValue ? jobsTrigger.GetPreviousFireTimeUtc().Value.ToString() : "No Information"
        //                    });
        //                }

        //                await _scheduler.ScheduleJob(jobsTriggerList?.FirstOrDefault());
        //            }
        //        }
        //        result = new Result<List<QuartzDto>>(ResultTypeEnum.Success, resultList, "Jobs successfully started.");
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<List<QuartzDto>>(false, $"An error occured while starting all jobs. Ex : {ex.ToString()}");
        //    }

        //    return result;
        //}
        //public static async Task<Result<List<QuartzDto>>> GetAllWorkingJobs()
        //{
        //    Result<List<QuartzDto>> result;
        //    List<QuartzDto> modelList = new List<QuartzDto>();
        //    try
        //    {
        //        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        //        IReadOnlyCollection<string> jobGroupList = await _scheduler.GetJobGroupNames();
        //        foreach (string jobGroup in jobGroupList)
        //        {
        //            GroupMatcher<JobKey> groupMatcher = GroupMatcher<JobKey>.GroupContains(jobGroup);
        //            IReadOnlyCollection<JobKey> jobKeys = await _scheduler.GetJobKeys(groupMatcher);
        //            foreach (var jobKey in jobKeys)
        //            {
        //                IJobDetail detail = await _scheduler.GetJobDetail(jobKey);
        //                IReadOnlyCollection<ITrigger> triggers = await _scheduler.GetTriggersOfJob(jobKey);
        //                foreach (ITrigger trigger in triggers)
        //                {

        //                    modelList.Add(new QuartzDto()
        //                    {
        //                        Group = jobGroup,
        //                        JobKeyName = jobKey.Name,
        //                        Description = detail.Description,
        //                        Name = trigger.Key.Name,
        //                        NextFireTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.LocalDateTime.ToString() : "No Information",
        //                        PreviousFireTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetPreviousFireTimeUtc().Value.ToString() : "No Information"
        //                    });
        //                }
        //            }
        //        }
        //        result = new Result<List<QuartzDto>>(modelList);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<List<QuartzDto>>(false, $"QuartzUtilizationService.GetAllWorkingJobs Method Ex : {ex.ToString()}");
        //    }

        //    return result;
        //}
        //private static IJobDetail CreateJobDetail<TJob>(string description = "") where TJob : IJob
        //{
        //    if (description.Equals(""))
        //    {
        //        _job = JobBuilder.Create<TJob>()
        //                        .WithIdentity(_jobName)
        //                        .Build();
        //    }
        //    else
        //    {
        //        _job = JobBuilder.Create<TJob>()
        //                        .WithIdentity(_jobName)
        //                        .WithDescription(description)
        //                        .Build();
        //    }

        //    return _job;
        //}
        //public static async Task<Result<string>> PauseAllJobs()
        //{
        //    Result<string> result;

        //    try
        //    {
        //        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //        await _scheduler.PauseAll();
        //        result = new Result<string>(true, "All jobs successfully paused.");
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<string>(true, $"Pausing all jobs failed. Ex : {ex.ToString()}");
        //    }

        //    return result;
        //}
        //public static async Task<Result<string>> ResumeAllJobs()
        //{
        //    Result<string> result;

        //    try
        //    {
        //        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //        await _scheduler.ResumeAll();
        //        result = new Result<string>(true, "All jobs successfully resumed.");
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<string>(true, $"Resuming all jobs failed. Ex : {ex.ToString()}");
        //    }

        //    return result;
        //}
        //public static async Task<Result<List<Result<JobDto>>>> InitializeAllJobs()
        //{
        //    Result<List<Result<JobDto>>> result;
        //    List<Result<JobDto>> innerResultList = new List<Result<JobDto>>();
        //    try
        //    {
        //        IJobService jobService = new JobService();
        //        List<JobDto> jobList = jobService.GetAllJobs().Data.Where(w => w.IsActive).ToList();
        //        if (jobList.Any())
        //        {
        //            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        //            await _scheduler.Start();
        //            foreach (JobDto job in jobList)
        //            {
        //                innerResultList.Add(await TriggerJob<SimpleTestProcess>(job));
        //            }
        //            result = new Result<List<Result<JobDto>>>(innerResultList);
        //        }
        //        else
        //        {
        //            result = new Result<List<Result<JobDto>>>(false, ResultTypeEnum.Warning, "Initializing jobs failed. There is no active jobs.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new Result<List<Result<JobDto>>>(false, $"An error occured while initializing jobs. Ex : {ex.ToString()}");
        //    }
        //    return result;
        //}
    }
}
