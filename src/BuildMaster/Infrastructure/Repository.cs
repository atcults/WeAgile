using System;
using System.Collections.Generic;
using System.Linq;
using BuildMaster.Model;
using Microsoft.EntityFrameworkCore;

namespace BuildMaster.Infrastructure
{
    public interface IRepository
    {
        void EnsureSeedData();
        IList<Job> GetAllJobs();

        Job GetJobWithTasks(int jobId);
        void AddJobToQueue(int jobId);

        JobQueue GetRecentJobQueue(int jobId);

        void UpdateJobQueue(long id, JobStatus jobStatus);
    }

    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            Console.WriteLine("Getting Repository");
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
                job.CheckVCS = true;
                job.TriggerTime = 1;
                job.RootLocation = "/Code/GitIntegration";

                List<JobTask> jobTasks = new List<JobTask>();

                jobTasks.AddRange(new[]{
                    new JobTask{
                        TaskOrder = 0,
                        TaskName = "Restore Project",
                        CommandName = "dotnet",
                        CommandAruments = "restore",
                        RelativePath = "/src/GitIntegration"
                    },
                    new JobTask{
                        TaskOrder = 1,
                        TaskName = "Build Project",
                        CommandName = "dotnet",
                        CommandAruments = "build",
                        RelativePath = "/src/GitIntegration"
                    },
                    new JobTask{
                        TaskOrder = 2,
                        TaskName = "Restore Test",
                        CommandName = "dotnet",
                        CommandAruments = "restore",
                        RelativePath = "/test/IntegrationTest"
                    },
                    new JobTask{
                        TaskOrder = 3,
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

        public Job GetJobWithTasks(int jobId)
        {
            return _context.Jobs.Include(x => x.JobTasks).FirstOrDefault(x => x.Id == jobId);
        }

        public JobQueue GetRecentJobQueue(int jobId)
        {
            return _context.JobQueues.Where(x => x.JobId == jobId).OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public JobQueue GetJobQueueById(long id)
        {
            return _context.JobQueues.Include(x=>x.Job).Where(x=> x.Id == id).First();
        }

        public void UpdateJobQueue(long id, JobStatus jobStatus)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingQueue = GetJobQueueById(id);
                    
                    if(jobStatus == JobStatus.Running)
                    {
                        existingQueue.StartTime = DateTime.UtcNow;
                    }
                    else if(jobStatus == JobStatus.Skipped)
                    {
                        existingQueue.FinishTime = DateTime.UtcNow;
                    }
                    else
                    {
                        existingQueue.FinishTime = DateTime.UtcNow;
                    }

                    existingQueue.JobStatus = jobStatus;

                    _context.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
        }

        public void AddJobToQueue(int jobId)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingQueue = GetRecentJobQueue(jobId);
                    if (existingQueue != null && existingQueue.JobStatus == JobStatus.Queued)
                    {
                        return;
                    }

                    _context.JobQueues.Add(new JobQueue
                    {
                        JobId = jobId,
                        JobStatus = JobStatus.Queued,
                        QueuedTime = DateTime.UtcNow
                    });

                    _context.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
        }
    }
}