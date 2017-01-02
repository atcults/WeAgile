using System;

namespace BuildMaster.Model
{
    public class JobQueue
    {
        public JobStatus JobStatus {get; set;}
        public DateTime QueuedTime {get; set;}
        public DateTime StartTime {get; set;}
        public DateTime FinishTime {get; set;}
        public int ExitCode { get; set; }
    }
}