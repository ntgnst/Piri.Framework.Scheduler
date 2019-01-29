using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Piri.Framework.Scheduler.Quartz.Service;
using System;
using System.Collections.Generic;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            IJobService jobService = new JobService();
            IScheduleJob scheduler = new QuartzService(jobService);
            Guid guid = Guid.NewGuid();
            string jobName = $"{typeof(TestJob).FullName}-{guid}";
            JobDto jobDto = new JobDto()
            {
                Guid = guid,
                IsActive = true,
                IsRunning = false,
                JobDataDtoList = new List<JobDataDto>()
                {
                    new JobDataDto()
                    {
                        Header = "testo,mesto;",
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
            Result<QuartzDto> result = scheduler.StartJob<TestJob>(jobDto, true).GetAwaiter().GetResult();

            //Result<QuartzDto> result = SchedulerUtilities.AddJob<SimpleTestProcess>("0/1 * * * * ? ", "This is a testing job.").GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result);
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
