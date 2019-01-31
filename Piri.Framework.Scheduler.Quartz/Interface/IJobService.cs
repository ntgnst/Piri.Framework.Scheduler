using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IJobService
    {
        Task<Result<List<JobDto>>> GetAllJobs();
        Task<Result<JobDto>> GetJobById(int jobId);
        Task<Result<string>> DeleteJob(string guid);
        Task<Result<JobDto>> UpdateJob(JobDto jobDto);
        Task<Result<JobDto>> AddJob(JobDto jobDto);
        Task<Result<JobDto>> GetJobByName(string jobName);
    }
}
