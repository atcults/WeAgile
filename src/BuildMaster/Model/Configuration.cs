using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    [Table("Configurations")]
    public class Configuration
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}