using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BuildMaster.Infrastructure;
using System;
using BuildMaster.VersionControl;
using BuildMaster.Model;

namespace BuildMaster
{
    public class TaskManager
    {
        private readonly IDictionary<string, Task> _taskCollecton = new Dictionary<string, Task>();

        private bool _stopSignal = false;
        private object _lock = new object();

        private int _poolInterval = 5000;

        private void QueueProcess()
        {
            while (!_stopSignal)
            {
                var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

                var jobs = repository.GetAllJobs();

                System.Console.WriteLine("Queue Jobs:" + jobs.Count);

                foreach (var job in jobs)
                {
                    var jobName = job.Name;

                    if (_taskCollecton.ContainsKey(jobName)) continue;

                    var lastJobQueue = repository.GetRecentJobQueue(job.Id);

                    if (lastJobQueue == null)
                    {
                        repository.AddJobToQueue(job.Id);
                        continue;
                    }

                    var elepsedTime = System.DateTime.UtcNow - lastJobQueue.QueuedTime;

                    var timeSpan = System.TimeSpan.FromMinutes(job.TriggerTime);

                    Console.WriteLine($"Time diff: {elepsedTime} - {timeSpan}");

                    if (elepsedTime < timeSpan)
                    {
                        continue;
                    }

                    repository.AddJobToQueue(job.Id);

                }

                Thread.Sleep(_poolInterval);
            }
        }

        private void JobRunner()
        {
            while (!_stopSignal)
            {
                var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

                var jobs = repository.GetAllJobs();

                System.Console.WriteLine("Runner Jobs:" + jobs.Count);

                foreach (var job in jobs)
                {
                    var jobName = job.Name;

                    if (_taskCollecton.ContainsKey(jobName)) continue;

                    var lastJobQueue = repository.GetRecentJobQueue(job.Id);

                    if (lastJobQueue != null && lastJobQueue.JobStatus == JobStatus.Queued)
                    {
                        Console.WriteLine($"Job: {job.Id}, Queue: {lastJobQueue.JobStatus}");

                        repository.UpdateJobQueue(lastJobQueue.Id, JobStatus.Running);
                        var task = Task.Factory.StartNew(() => RunJob(job.Id)).ContinueWith(tsk => _taskCollecton.Remove(job.Name));
                        _taskCollecton.Add(job.Name, task);
                    }
                }

                Thread.Sleep(_poolInterval);
            }
        }

        private void RunJob(long jobId)
        {
            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            var job = repository.GetJobWithTasks(jobId);

            var jobQueue = repository.GetRecentJobQueue(jobId);

            if (job.CheckVCS)
            {
                Console.WriteLine("Checking incoming changes");
                var gitProcessor = GitProcessor.GetProcessorForPath(job.RootLocation);
                if (!gitProcessor.HasReceivedIncomingChanges)
                {
                    Console.WriteLine("No incoming changes, Skipping...");
                    repository.UpdateJobQueue(jobQueue.Id, JobStatus.Skipped);
                    return;
                }
            }

            var status = JobStatus.Success;

            foreach(var jobTask in job.JobTasks)
            {
                var code = ProcessRunner.RunProcess(jobTask.CommandName, jobTask.CommandAruments, job.RootLocation + jobTask.RelativePath);
                if(code != 0)
                {
                    status = JobStatus.Failed;
                    break;
                }
            }

            repository.UpdateJobQueue(jobQueue.Id, status);

            Console.WriteLine($"Job {job.Name} finished");
        }

        public void Start()
        {
            ServiceCollectionProvider.Instance.Provider.GetService<ApplicationDbContext>().Database.Migrate();

            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.EnsureSeedData();

            _taskCollecton.Add("queue", Task.Factory.StartNew(() => QueueProcess()));
            _taskCollecton.Add("process", Task.Factory.StartNew(() => JobRunner()));
        }

        public void Stop()
        {
            _stopSignal = true;
        }

        public void WaitForAll()
        {
            Task.WaitAll(_taskCollecton.Values.ToArray());
        }
    }
}