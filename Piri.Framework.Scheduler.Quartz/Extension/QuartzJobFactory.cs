using Piri.Framework.Scheduler.Quartz.Job;
using Quartz;
using Quartz.Spi;
using System;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public class PiriJobFactory : IPiriJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PiriJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle firedBundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = firedBundle.JobDetail;
            IJob job = (IJob)_serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job) { }
    }
}
