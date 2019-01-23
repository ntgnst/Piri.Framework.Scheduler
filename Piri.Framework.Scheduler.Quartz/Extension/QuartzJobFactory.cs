using Quartz;
using Quartz.Spi;
using System;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QuartzJobFactory(IServiceProvider serviceProvider)
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
