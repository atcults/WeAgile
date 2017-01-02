using System;
using System.Collections.Generic;
using System.Linq;
using BuildMaster.Model;

namespace BuildMaster.Infrastructure
{
    public interface IRepository
    {
        void EnsureSeedData();
        IList<Job> GetAllJobs();
    }

    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            if (_context.Configurations.Any())
                return;

            using (var trn = _context.Database.BeginTransaction())
            {
                _context.Configurations.AddRange(new[] {
                    new Configuration
                    {
                        Key = "version",
                        Value = "0.1"
                    },
                    new Configuration
                    {
                        Key = "banner",
                        Value = "Build Master"
                    },
                    new Configuration
                    {
                        Key = "temp path",
                        Value = "/temp"
                    },
                    new Configuration
                    {
                        Key = "tools path",
                        Value = "/environment"
                    },
                    new Configuration
                    {
                        Key = "log path",
                        Value = "/temp/logs"
                    },
                    new Configuration
                    {
                        Key = "status",
                        Value = "stopped"
                    }
                });

                var job = new Job();

                job.Name = "Git Interation";
                job.RootLocation = "/Code/GitIntegration";

                List<Job.Task> jobTasks = new List<Job.Task>();

                jobTasks.AddRange(new[]{
                    new Job.Task{
                        TaskName = "Restore Project",
                        CommandName = "dotnet",
                        CommandAruments = "restore",
                        RelativePath = "/src/GitIntegration"
                    },
                    new Job.Task{
                        TaskName = "Build Project",
                        CommandName = "dotnet",
                        CommandAruments = "build",
                        RelativePath = "/src/GitIntegration"
                    },
                    new Job.Task{
                        TaskName = "Restore Test",
                        CommandName = "dotnet",
                        CommandAruments = "restore",
                        RelativePath = "/test/IntegrationTest"
                    },
                    new Job.Task{
                        TaskName = "Integration Test",
                        CommandName = "dotnet",
                        CommandAruments = "test",
                        RelativePath = "/test/IntegrationTest"
                    }
                });

                job.JobTasks = jobTasks;

                _context.Jobs.Add(job);

                _context.SaveChanges();

                trn.Commit();
            }
        }

        public IList<Job> GetAllJobs()
        {
            return _context.Jobs.ToList();
        }
    }
}