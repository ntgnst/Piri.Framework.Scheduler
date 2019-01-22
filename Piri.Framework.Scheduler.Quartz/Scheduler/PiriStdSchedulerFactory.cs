using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Piri.Framework.Scheduler.Quartz.Scheduler
{
    public class PiriStdSchedulerFactory : StdSchedulerFactory
    {
        public override Task<IReadOnlyList<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetAllSchedulers(cancellationToken);
        }

        public override Task<IScheduler> GetScheduler(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetScheduler(cancellationToken);
        }

        public override Task<IScheduler> GetScheduler(string schedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.GetScheduler(schedName, cancellationToken);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Initialize(NameValueCollection props)
        {
            base.Initialize(props);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
