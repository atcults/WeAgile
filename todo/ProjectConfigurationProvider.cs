using System;
using System.IO;
using Newtonsoft.Json;

namespace GitIntegration
{
    public class ProjectConfigurationProvider
    {
        private const string ConfigFile = "ProjectDetails.json";
        private static readonly Lazy<ProjectConfiguration> Lazy = new Lazy<ProjectConfiguration>(() =>
        {
            var details = ReadConfigurations();

            return details;
        });

        public static ProjectConfiguration Instance => Lazy.Value;

        private ProjectConfigurationProvider()
        {
        }

        private static ProjectConfiguration ReadConfigurations()
        {
            if (!File.Exists(ConfigFile))
            {
                var details = new ProjectConfiguration();
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(details));
            }

            var str = File.ReadAllText(ConfigFile);
            return JsonConvert.DeserializeObject<ProjectConfiguration>(str);
        }

        public static void ConfigureSeedServices()
        {
            if (File.Exists(ConfigFile)) return;

            var config = Instance;

            config.ToolsPath = "C:/Environment/tools";
            config.TempPath = "C:/temp";
            config.AddProject("jayhawk.com", "C:/Projects/jayhawk");
            config.AddProject("libcloud.com", "C:/Projects/LibCloud");

            UpdateConfiguration();
        }

        public static void UpdateConfiguration()
        {
            File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(Instance));
        }
    }
}