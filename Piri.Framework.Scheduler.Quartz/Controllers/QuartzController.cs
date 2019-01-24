using Microsoft.AspNetCore.Mvc;
using Piri.Framework.Scheduler.Quartz.Domain;
using Piri.Framework.Scheduler.Quartz.Extension;
using Piri.Framework.Scheduler.Quartz.Interface.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    [Route("api/quartz")]
    public class QuartzController : Controller
    {
        [Route("GetList")]
        public async Task<JsonResult> GetList()
        {
            Result<List<QuartzDto>> result = await QuartzServiceUtilities.GetAllWorkingJobs();
            return Json(result);
        }
        [Route("StartAllJobs")]
        public async Task<JsonResult> StartAllJobs()
        {
            Result<QuartzDto> result = await QuartzServiceUtilities.StartAll();
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetJobDetail(string jobName = "", string regex = "", bool isStartNow = false)
        {
            return Json("");
        }

        [HttpGet]
        [Route("PauseAll")]
        public async Task<JsonResult> PauseAll()
        {
           Result<string> result = await QuartzServiceUtilities.PauseAllJobs();
            return Json(result);
        }

        [HttpGet]
        [Route("ResumeAll")]
        public async Task<JsonResult> ResumeAll()
        {
            Result<string> result = await QuartzServiceUtilities.ResumeAllJobs();
            return Json(result);
        }
    }
}