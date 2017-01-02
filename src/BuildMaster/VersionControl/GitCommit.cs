using System.Collections.Generic;
using System.Text;

namespace BuildMaster.VersionControl
{
    public class GitCommit
    {
        public GitCommit()
        {
            Headers = new Dictionary<string, string>();
            Files = new List<GitFileStatus>();
            Message = "";
        }

        public Dictionary<string, string> Headers { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }
        public List<GitFileStatus> Files { get; set; }

        public override string ToString()
        {
            var outString = new StringBuilder();

            outString.AppendLine("commit " + Sha);

            foreach (var key in Headers.Keys)
            {
                outString.AppendLine(key + ":" + Headers[key]);
            }

            outString.AppendLine(Message);

            foreach (var file in Files)
            {
                outString.AppendLine(file.Status + "\t" + file.File);
            }

            return outString.ToString();
        }
    }
}