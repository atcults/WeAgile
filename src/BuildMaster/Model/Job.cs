using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BuildMaster.Model
{
    [Table("Jobs")]
    public class Job : Entity
    {
        public string Name { get; set; }
        public string RootLocation { get; set; }
        public string Configuration { get; set; }
        public bool CheckVCS { get; set; }
        public int TriggerTime { get; set; }

        [NotMapped]
        public IList<Task> JobTasks
        {
            get
            {
                if (string.IsNullOrEmpty(Configuration))
                {
                    return new List<Task>();
                }
                return JsonConvert.DeserializeObject<List<Task>>(Configuration);
            }
            set
            {
                Configuration = JsonConvert.SerializeObject(value);
            }
        }

        public class Task
        {
            public string TaskName { get; set; }
            public string CommandName { get; set; }
            public string CommandAruments { get; set; }
            public string RelativePath { get; set; }
        }
    }
}