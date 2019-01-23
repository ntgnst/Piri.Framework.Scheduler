using Piri.Framework.Scheduler.Quartz.Interface;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    public class TestJob : IPiriQuartzJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //Logic
            Console.WriteLine("Just Throttling...");
            return Task.FromResult(0);
        }
    }
}
