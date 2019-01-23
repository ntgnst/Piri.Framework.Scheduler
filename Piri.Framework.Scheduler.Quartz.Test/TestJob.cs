using Piri.Framework.Scheduler.Quartz.Extension;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    public class TestJob : IPiriQuartz
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Just Throttling...");
            return Task.FromResult(0);
        }
    }
}
