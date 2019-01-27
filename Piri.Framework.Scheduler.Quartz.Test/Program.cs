using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
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
                Result<QuartzDto> result = QuartzServiceUtilities.AddJob<SimpleTestProcess>("0/1 * * * * ? ", "This is a testing job.").GetAwaiter().GetResult();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {

            }

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
