using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public static class QuartzExtension
    {
        static bool _result = false;
        public static bool CheckAndSaveQuartzJobs()
        {
            return _result;
        }
        public static void UseQuartz(this IServiceCollection services, params Type[] jobs)
        {
            services.AddSingleton<IJobFactory, QuartzJobFactory>();
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));
            services.AddSingleton(provider =>
            {
                StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
                IScheduler scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                return scheduler;
            });
        }
    }
}
