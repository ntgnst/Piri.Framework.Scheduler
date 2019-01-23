using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Piri.Framework.Scheduler.Quartz.Scheduler
{
    public class PiriStdSchedulerFactory : IPiriSchedulerFactory
    {
        private readonly ISchedulerFactory _schedulerFactory;
        Task<IReadOnlyList<IScheduler>> ISchedulerFactory.GetAllSchedulers(CancellationToken cancellationToken)
        {
            return _schedulerFactory.GetAllSchedulers(cancellationToken);
        }

        Task<IScheduler> ISchedulerFactory.GetScheduler(CancellationToken cancellationToken)
        {
            return _schedulerFactory.GetScheduler(cancellationToken);
        }

        Task<IScheduler> ISchedulerFactory.GetScheduler(string schedName, CancellationToken cancellationToken)
        {
            return _schedulerFactory.GetScheduler(schedName, cancellationToken);
        }
    }
}
