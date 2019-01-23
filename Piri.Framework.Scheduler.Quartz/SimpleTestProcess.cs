using Quartz;
using System;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz
{
    public interface IPiriQuartz : IJob
    {

    }
    public class SimpleTestProcess : IPiriQuartz
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("I am runnin'. Rollin' , Rollin' , Rollin' , Rollin', Rollin'");
            return Task.FromResult(0);
        }
    }
}