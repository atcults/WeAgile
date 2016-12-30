namespace BuildMaster.Model
{
     public class JobTaskResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
    }
}