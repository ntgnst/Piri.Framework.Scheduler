using Microsoft.AspNetCore.Mvc;
using Piri.Framework.Scheduler.Quartz.Extension;

namespace Piri.Framework.Scheduler.Quartz.Controllers
{
    [Produces("application/json")]
    [Route("api/Quartz")]
    public class QuartzController : Controller
    {
        [HttpGet]
        public JsonResult GetWorkingJobs()
        {
            return Json("");
        }

        [HttpPost]
        public JsonResult SetJobDetail(string jobName = "", string regex = "", bool isStartNow = false)
        {

            return Json("");
        }
    }
}