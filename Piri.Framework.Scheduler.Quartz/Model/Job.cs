using System;
using System.Collections.Generic;

namespace Piri.Framework.Scheduler.Quartz.Model
{
    public partial class Job
    {
        public Job()
        {
            JobData = new HashSet<JobData>();
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime? LastRunTime { get; set; }
        public DateTime? LastEndTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        public bool IsPaused { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<JobData> JobData { get; set; }
    }
}
