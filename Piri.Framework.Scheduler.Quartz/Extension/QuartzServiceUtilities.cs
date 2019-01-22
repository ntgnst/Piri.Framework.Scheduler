using Piri.Framework.Scheduler.Quartz.Job;
using Piri.Framework.Scheduler.Quartz.Scheduler;
using Quartz;
using Quartz.Impl;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public class QuartzServiceUtilities
    {
        public static void StartJob<TJob>(/*ILoggerFactory loggerFactory, IQuartzServiceUtilities quartzServiceUtilities,*/ string timerRegex, bool isStartNow = true)
               where TJob : IPiriJob
        {
            //Logger<IJob> logger = new Logger<IJob>(loggerFactory);

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

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

            scheduler.ScheduleJob(job, trigger);

            //logger.LogInformation($"QuartzServicesUtilities.StartJob JobName : {jobName} started. TimerRegex : {timerRegex} IsStartNow : {isStartNow}");
        }
    }
}
