namespace BuildMaster.Model
{
     public class JobTask
    {
        public string TaskName { get; set; }
        public string CommandName { get; set; }
        public string CommandAruments { get; set; }
        public string WorkingPath { get; set; }
    }
}