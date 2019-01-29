using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piri.Framework.Scheduler.Quartz.Domain
{
    public class ScheduleDto
    {
        public string Cron { get; set; }
        public string MethodType { get; set; }
        public string Body { get; set; }
        public string Header { get; set; }
        public string RetryCount { get; set; }
    }
}
