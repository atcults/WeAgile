using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BuildMaster.Infrastructure;

namespace BuildMaster
{
    public class TaskManager
    {
        private readonly IDictionary<string, Task> _taskCollecton = new Dictionary<string, Task>();

        private bool _stopSignal = false;
        private object _lock = new object();

        private void RunMain()
        {
            ServiceCollectionProvider.Instance.Provider.GetService<ApplicationDbContext>().Database.Migrate();

            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.EnsureSeedData();

            var jobs = repository.GetAllJobs();

            System.Console.WriteLine("Jobs:" + jobs.Count);

            while (!_stopSignal)
            {
                foreach(var job in jobs)
                {
                    var jobName = job.Name;

                    System.Console.WriteLine("Job:" + jobName);

                    if(_taskCollecton.ContainsKey(jobName)) continue;
                    
                    var timeSpan = System.TimeSpan.FromMinutes(job.TriggerTime);

                    System.Console.WriteLine("Job Tasks" + job.JobTasks.Count);

                    foreach(var jobTask in job.JobTasks)
                    {
                        System.Console.WriteLine(jobTask.TaskName);
                    }
                }

                Thread.Sleep(5000);

                 //var gitProcessor = GitProcessor.GetProcessorForPath(job.RootLocation);                 
            }
        }

        public void Start()
        {
            _taskCollecton.Add("main", Task.Factory.StartNew(() => RunMain()));
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