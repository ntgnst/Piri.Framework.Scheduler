using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Interface;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IScheduleJob _scheduleJob;
        private readonly ILogger _logger;
        public MonitorController(IJobService jobService, IScheduleJob scheduleJob,ILogger<MonitorController> logger)
        {
            _jobService = jobService;
            _scheduleJob = scheduleJob;
            _logger = logger;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> List()
        {
            Result<List<JobDto>> result = await _scheduleJob.GetAllWorkingJobs();
            List<Result<JobDto>> jobResult = new List<Result<JobDto>>();
            foreach (var item in result.Data)
            {
                jobResult.Add(await _jobService.GetJobByName(item.JobDataDtoList.FirstOrDefault().Name));

            }
            _logger.LogInformation("/ replied :", jobResult);
            return View(jobResult);
        }
    }
}
