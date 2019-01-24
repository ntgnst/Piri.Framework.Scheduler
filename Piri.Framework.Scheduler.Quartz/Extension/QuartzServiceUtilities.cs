using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
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
    public static class QuartzServiceUtilities
    {
        private static IScheduler _scheduler;
        private static ICronTrigger _cronTrigger;
        private static IJobDetail _job;
        private static TriggerBuilder _triggerBuilder;
        private static ITrigger _trigger;
        private static string _jobName;

        //Creates and starts a job
        public static async void StartJob<TJob>(string timerRegex, bool isStartNow = false)
               where TJob : IJob
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            await _scheduler.Start();

            _jobName = typeof(TJob).FullName;

            _job = JobBuilder.Create<TJob>()
                .WithIdentity(_jobName)
                .Build();

            if (isStartNow)
            {
                _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity($"{_jobName}.trigger")
                .StartNow()
                .WithCronSchedule(timerRegex)
                .Build();
            }
            else
            {
                _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity($"{_jobName}.trigger")
                .WithCronSchedule(timerRegex)
                .Build();
            }

            await _scheduler.ScheduleJob(_job, _cronTrigger);
        }
        public static async void TriggerJob<TJob>(string jobName, string cronRegex) where TJob : IJob
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();

            _job = JobBuilder.Create()
                .WithIdentity(jobName)
                .Build();

            _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity($"{_jobName}.trigger")
                .WithCronSchedule(cronRegex)
                .Build();


            _triggerBuilder = TriggerBuilder.Create();

            _trigger = _triggerBuilder
                            .ForJob(_job.Key)
                            .WithCronSchedule(cronRegex)
                            .WithIdentity(jobName)
                            .Build();

            await _scheduler.RescheduleJob(_cronTrigger.Key, _trigger);
        }
        public static async Task<Result<QuartzDto>> StartAll()
        {
            //TODO : Logic Get All Registered Jobs And Fire Them.
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }
            return new Result<QuartzDto>();

        }
        //Adds new Job Without starting
        public static async Task<Result<QuartzDto>> AddJob<TJob>(string timerRegex, string description = "") where TJob : IJob
        {
            Result<QuartzDto> result;
            try
            {
                _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                await _scheduler.Start();
                _jobName = typeof(TJob).FullName;
                _job = CreateJobDetail<TJob>(description);

                if (await _scheduler.CheckExists(_job.Key))
                {
                    result = new Result<QuartzDto>(ResultTypeEnum.Error, null, "Given JobKey is already exist.");
                }
                else
                {
                    _cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                                    .WithIdentity($"{_jobName}.trigger")
                                    .WithCronSchedule(timerRegex)
                                    .Build();

                    await _scheduler.ScheduleJob(_job, _cronTrigger);
                    result = new Result<QuartzDto>(new QuartzDto() { Name = _job?.Key.ToString(), Description = _job?.Description, JobKeyName = _job.Key.ToString() });
                }
            }
            catch (Exception ex)
            {
                result = new Result<QuartzDto>(false, ex.ToString());
            }

            return result;
        }
        public static async Task<Result<List<QuartzDto>>> GetAllWorkingJobs()
        {
            Result<List<QuartzDto>> result;
            List<QuartzDto> modelList = new List<QuartzDto>();
            try
            {
                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                IReadOnlyCollection<string> jobGroups = await scheduler.GetJobGroupNames();
                foreach (string group in jobGroups)
                {
                    GroupMatcher<JobKey> groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                    IReadOnlyCollection<JobKey> jobKeys = await scheduler.GetJobKeys(groupMatcher);
                    foreach (var jobKey in jobKeys)
                    {
                        IJobDetail detail = await scheduler.GetJobDetail(jobKey);
                        IReadOnlyCollection<ITrigger> triggers = await scheduler.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {

                            modelList.Add(new QuartzDto()
                            {
                                Group = group,
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
        private static IJobDetail CreateJobDetail<TJob>(string description = "") where TJob : IJob
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
        public static async Task<Result<string>> PauseAllJobs()
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
        public static async Task<Result<string>> ResumeAllJobs()
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
    }
}
