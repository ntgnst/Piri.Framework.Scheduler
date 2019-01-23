using Piri.Framework.Scheduler.Quartz.Extension;
using System;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            QuartzServiceUtilities.StartJob<TestJob>("0/5 * * * * ? ", true);
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
