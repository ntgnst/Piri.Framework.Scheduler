using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Piri.Framework.Scheduler.Quartz.Job;
using Piri.Framework.Scheduler.Quartz.Scheduler;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public static class QuartzExtension
    {
        public static void UseQuartz(this IServiceCollection services, params Type[] jobs)
        {
            services.AddSingleton<IPiriJobFactory, PiriJobFactory>();
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton)));
            services.AddSingleton(provider =>
            {
                PiriStdSchedulerFactory schedulerFactory = new PiriStdSchedulerFactory();
                IPiriScheduler scheduler = schedulerFactory.GetScheduler().Result as IPiriScheduler;
                scheduler.JobFactory = provider.GetService<IPiriJobFactory>();
                return scheduler;
            });
        }
    }
}
