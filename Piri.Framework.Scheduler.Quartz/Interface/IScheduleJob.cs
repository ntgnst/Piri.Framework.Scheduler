using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IScheduleJob
    {
        //Task<Result<QuartzDto>> StartJob<TJob>(JobDto jobDto, bool isStartNow = false) where TJob : IJob;
        Task<Result<JobDto>> TriggerJob<TJob>(JobDto jobDto) where TJob : IJob;
        Task<Result<QuartzDto>> AddJob<TJob>(JobDto jobDto, string description = "", bool isStartNow = false) where TJob : IJob;
        Task<Result<List<JobDto>>> GetAllWorkingJobs();
        Task<Result<string>> PauseAllJobs();
        Task<Result<string>> ResumeAllJobs();
        Task<Result<List<Result<JobDto>>>> InitializeAllJobs();
    }
}
