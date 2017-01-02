using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    [Table("Jobs")]
    public class Job : Entity
    {
        public string Name { get; set; }
        public string RootLocation { get; set; }
        public bool CheckVCS { get; set; }
        public int TriggerTime { get; set; }
        public virtual ICollection<JobTask> JobTasks { get; set; }
    }

    [Table("JobTasks")]
    public class JobTask : Entity
    {
        [ForeignKey("JobdRefId")]
        public virtual Job Job { get; set; }
        [Required]
        public int TaskOrder { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string CommandName { get; set; }
        public string CommandAruments { get; set; }
        public string RelativePath { get; set; }
    }
}