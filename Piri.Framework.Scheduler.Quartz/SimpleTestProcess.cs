using System.Threading.Tasks;
using Piri.Framework.Scheduler.Quartz.Job;
using Quartz;
using System;

namespace Piri.Framework.Scheduler.Quartz
{
    public class SimpleTestProcess : IPiriJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("I am runnin'");
            return Task.FromResult(0);
        }
    }
}