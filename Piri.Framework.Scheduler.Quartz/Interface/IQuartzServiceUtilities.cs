using Piri.Framework.Scheduler.Quartz.Job;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IQuartzServiceUtilities
    {
        void StartJob<TJob>(string timerRegex, bool isStartNow = true) where TJob : IPiriJob;
    }
}
