using Piri.Framework.Scheduler.Quartz.Extension;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    public class TestJob : PiriJob
    {
        public override async Task StartAsync(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Just Throttling...");
        }
    }
}
