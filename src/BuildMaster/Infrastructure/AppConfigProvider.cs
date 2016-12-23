using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BuildMaster.Infrastructure
{
    public class AppConfigProvider
    {
        private const string ConfigFile = "appsettings.json";
        private IConfigurationRoot Configuration { get; set; }
      
        private static readonly Lazy<AppConfigProvider> Lazy = new Lazy<AppConfigProvider>(() =>
        {
            return new AppConfigProvider();
        });

        public static AppConfigProvider Instance => Lazy.Value;

        private AppConfigProvider()
        {
        }

        public void Configure(string[] args)
        {
             Configuration = new ConfigurationBuilder()
                           .AddCommandLine(args)
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile($"appsettings.json", optional: false)
                           .AddEnvironmentVariables()
                           .Build();
        }

        public string GetConnectionString()
        {
            if(Configuration == null)
            {
                Configure(new string[]{});
            }

            return Configuration.GetConnectionString("DefaultConnection");
        }
        
    }
}