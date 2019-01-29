using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System.Collections.Generic;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IJobService
    {
        Result<List<JobDto>> GetAllJobs();
        Result<JobDto> GetJobById(int jobId);
        Result<bool> DeleteJob(int jobId);
        Result<JobDto> UpdateJob(JobDto jobDto);
        Result<JobDto> AddJob(JobDto jobDto);
    }
}
