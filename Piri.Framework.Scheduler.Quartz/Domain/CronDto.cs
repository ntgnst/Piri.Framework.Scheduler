namespace Piri.Framework.Scheduler.Quartz.Domain
{
    public class CronDto
    {
        public string _cronRegex { get; set; }
        public CronDto(string cronRegex)
        {
            _cronRegex = cronRegex;
        }
        public CronDto()
        {
        }
    }
}
