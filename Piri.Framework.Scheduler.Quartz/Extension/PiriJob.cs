using Piri.Framework.Scheduler.Quartz.Interface;
using Quartz;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Extension
{
    public abstract class PiriJob : IPiriJob
    {
        public virtual Task Execute(IJobExecutionContext context)
        {
            StartAsync(context);
            return Task.FromResult(0);
        }
        public abstract Task StartAsync(IJobExecutionContext context);

    }
}
