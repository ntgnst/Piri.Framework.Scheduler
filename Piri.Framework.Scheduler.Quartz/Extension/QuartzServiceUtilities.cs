using Quartz;
using Quartz.Impl;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public static class QuartzServiceUtilities 
    {
        public static async void StartJob<TJob>(string timerRegex, bool isStartNow = false)
               where TJob : IJob
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            await scheduler.Start();

            string jobName = typeof(TJob).FullName;

            IJobDetail job = JobBuilder.Create<TJob>()
                .WithIdentity(jobName)
                .Build();

            ICronTrigger trigger;
            if (isStartNow)
            {
                trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger")
                .StartNow()
                .WithCronSchedule(timerRegex)
                .Build();
            }
            else
            {
                trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity($"{jobName}.trigger")
                .WithCronSchedule(timerRegex)
                .Build();
            }

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
