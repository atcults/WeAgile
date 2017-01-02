using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    [Table("JobTasks")]
    public class JobTask : Entity
    {
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        public int TaskOrder { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string CommandName { get; set; }
        public string CommandAruments { get; set; }
        public string RelativePath { get; set; }
    }
}