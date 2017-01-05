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

        private void QueueProcess()
        {
            ServiceCollectionProvider.Instance.Provider.GetService<ApplicationDbContext>().Database.Migrate();

            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.EnsureSeedData();

            while (!_stopSignal)
            {
                var jobs = repository.GetAllJobs();

                System.Console.WriteLine("Jobs:" + jobs.Count);

                foreach (var job in jobs)
                {
                    var jobName = job.Name;

                    if (_taskCollecton.ContainsKey(jobName)) continue;

                    var lastJobQueue = repository.GetRecentJobQueue((int)job.Id);

                    if (lastJobQueue == null)
                    {
                        repository.AddJobToQueue((int)job.Id);
                        continue;
                    }

                    System.Console.WriteLine("Job:" + jobName);

                    var elepsedTime = System.DateTime.UtcNow - lastJobQueue.QueuedTime;

                    var timeSpan = System.TimeSpan.FromMinutes(job.TriggerTime);

                    if (elepsedTime < timeSpan)
                    {
                        continue;
                    }

                    if (job.CheckVCS)
                    {
                        var gitProcessor = GitProcessor.GetProcessorForPath(job.RootLocation);
                        if (gitProcessor.HasReceivedIncomingChanges)
                        {
                            repository.AddJobToQueue((int)job.Id);
                        }
                    }
                    else
                    {
                        repository.AddJobToQueue((int)job.Id);
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private void JobRunner()
        {
            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            while (!_stopSignal)
            {
                var jobs = repository.GetAllJobs();

                System.Console.WriteLine("Jobs:" + jobs.Count);

                foreach (var job in jobs)
                {
                    var jobName = job.Name;

                    if (_taskCollecton.ContainsKey(jobName)) continue;

                    var lastJobQueue = repository.GetRecentJobQueue((int)job.Id);

                    if(lastJobQueue != null || lastJobQueue.JobStatus == JobStatus.Queued)
                    {
                        _taskCollecton.Add(job.Name, Task.Factory.StartNew(() => RunJob(job.Name, (int)job.Id)));
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private void RunJob(string jobName, long jobQueueId)
        {
            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.UpdateJobQueue(jobQueueId, JobStatus.Running);

            Console.WriteLine("Job Started");

            System.Threading.Thread.Sleep(60000);

            repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.UpdateJobQueue(jobQueueId, JobStatus.Success);

            Console.WriteLine("Job Finished");

            _taskCollecton.Remove(jobName);
        }

        public void Start()
        {
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