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
        public TriggerType TriggerType { get; set; }

        public int TriggerTime {get; set;}

        [NotMapped]
        public IList<JobTask> JobTasks
        {
            get
            {
                if (string.IsNullOrEmpty(Configuration))
                {
                    return new List<JobTask>();
                }
                return JsonConvert.DeserializeObject<List<JobTask>>(Configuration);
            }
            set
            {
                Configuration = JsonConvert.SerializeObject(value);
            }
        }
    }
}