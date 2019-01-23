using Piri.Framework.Scheduler.Quartz.Interface;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz
{
    public class SimpleTestProcess : IPiriQuartzJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            //Logic
            Console.WriteLine("I am runnin'. Rollin' , Rollin' , Rollin' , Rollin', Rollin'");
            return Task.FromResult(0);
        }
    }
}