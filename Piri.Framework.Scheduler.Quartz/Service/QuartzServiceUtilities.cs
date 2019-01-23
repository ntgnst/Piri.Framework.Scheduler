using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Service
{
    public class QuartzServiceUtilities : IQuartzServiceUtilities
    {
        public async Task<Result<List<QuartzDto>>> GetAllWorkingJobs()
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
    }
}
