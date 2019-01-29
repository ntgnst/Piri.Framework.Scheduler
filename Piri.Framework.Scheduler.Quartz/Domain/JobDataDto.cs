namespace Piri.Framework.Scheduler.Quartz.Domain
{
    public class JobDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TimerRegex { get; set; }
        public string Header { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Method { get; set; }
        public bool IsRetry { get; set; }
        public int? RetryInterval { get; set; }
        public int? RetryCount { get; set; }
        public int JobId { get; set; }

        public JobDto JobDto { get; set; }
    }
}
