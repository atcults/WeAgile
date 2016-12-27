using System.ComponentModel.DataAnnotations.Schema;

namespace BuildMaster.Model
{
    public abstract class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}