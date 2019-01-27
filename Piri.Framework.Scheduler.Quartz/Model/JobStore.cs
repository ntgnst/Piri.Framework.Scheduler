using System;

namespace Piri.Framework.Scheduler.Quartz.Model
{
    public partial class JobStore
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string TimerRegex { get; set; }
        public Guid Guid { get; set; }
        public bool IsOnlyInsert { get; set; }
        public bool IsRunning { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastRunTime { get; set; }
        public DateTime LastEndTime { get; set; }
        public DateTime LastStartTime { get; set; }
        public string TriggerKeyName { get; set; }
    }
}
