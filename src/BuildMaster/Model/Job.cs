using System.Collections.Generic;
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
        public virtual ICollection<JobQueue> JobQueues {get; set;}
    }
}