using System.Collections.Generic;

namespace GitIntegration
{
    public class ProjectConfiguration
    {
        public string ToolsPath { get; set; }

        public string TempPath { get; set; }
        public List<ProjectDetail> Projects { get; set; }

        public ProjectConfiguration()
        {
            Projects = new List<ProjectDetail>();
        }

        public void AddProject(string name, string location)
        {
            Projects.Add(new ProjectDetail
            {
                Name = name,
                Location = location
            });
        }
    }
}