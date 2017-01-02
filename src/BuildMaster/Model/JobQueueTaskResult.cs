using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    [Table("JobQueueTaskResults")]
    public class JobQueueTaskResult : Entity
    {
        [ForeignKey("JobQueueId")]
        public virtual JobQueue JobQueue { get; set; }
        public int TaskOrder { get; set; }
        public string TaskName { get; set; }
        [Required]
        public string CommandName { get; set; }
        public string CommandAruments { get; set; }
        public string WorkingDirectory { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int? ExitCode { get; set; }
    }
}