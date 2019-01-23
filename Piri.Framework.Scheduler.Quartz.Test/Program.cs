using Piri.Framework.Scheduler.Quartz.Extension;
using System;

namespace Piri.Framework.Scheduler.Quartz.Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            QuartzServiceUtilities.StartJob<TestJob>("0/1 * * * * ?", true);
            try
            {
                QuartzServiceUtilities.AddJob<TestJob>("0/1 * * * * ? ", "This is a testing job.");
            }
            catch (Exception ex)
            {
                
            }
            
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
