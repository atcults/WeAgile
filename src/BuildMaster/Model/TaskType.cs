namespace BuildMaster.Model
{
    public enum TaskType
    {
        Repository,
        ShellCommand
    }

    public class JobTask
    {
        public TaskType TaskType { get; set; }
        public string TaskName { get; set; }
        public string CommandName { get; set; }
        public string CommandAruments { get; set; }
        public string WorkingPath {get; set;}
    }

    public class JobTaskResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
    }
}