using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Interface
{
    public interface IQuartzServiceUtilities
    {
        Task<Result<List<QuartzDto>>> GetAllWorkingJobs();
    }
}
