using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    [Table("JobQueues")]
    public class JobQueue : Entity
    {
        public long JobId {get; set;}
        
        [ForeignKey("JobId")]
        public Job Job {get; set;}
        public DateTime QueuedTime {get; set;}
        public DateTime? StartTime {get; set;}
        public DateTime? FinishTime {get; set;}
        public JobStatus JobStatus {get; set;}
    }
}