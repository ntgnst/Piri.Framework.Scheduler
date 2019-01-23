namespace Piri.Framework.Scheduler.Quartz.Domain
{
    public class QuartzDto
    {
        public string Group { get; set; }
        public string JobKeyName { get; set; }
        public string JobKeyGroup { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NextFireTime { get; set; }
        public string PreviousFireTime { get; set; }
    }
}
