using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using BuildMaster.Infrastructure;
using BuildMaster.Model;

namespace BuildMaster
{
    public class TaskManager
    {
        private readonly IDictionary<string, Task> _taskCollecton = new Dictionary<string, Task>();
        private readonly IDictionary<string, List<JobQueue>> _jobQueues = new Dictionary<string, List<JobQueue>>();

       // private readonly IDictionary<string, 

        private bool _stopSignal = false;
        private object _lock = new object();

        private void RunMain()
        {
            ServiceCollectionProvider.Instance.Provider.GetService<ApplicationDbContext>().Database.Migrate();

            var repository = ServiceCollectionProvider.Instance.Provider.GetService<IRepository>();

            repository.EnsureSeedData();

            var jobs = repository.GetAllJobs();

            foreach(var job in jobs)
            {
                _jobQueues.Add(job.Name, new List<JobQueue>());
            }

            while (!_stopSignal)
            {
                foreach(var job in jobs)
                {
                    var jobName = job.Name;

                    if(_taskCollecton.ContainsKey(jobName)) continue;
                    
                    var timeSpan = System.TimeSpan.FromMinutes(job.TriggerTime);

                    

                    var JobQueue = _jobQueues[jobName].LastOrDefault();
                    //if(JobQueue == null) 




                }

                Thread.Sleep(5000);

                 //var gitProcessor = GitProcessor.GetProcessorForPath(job.RootLocation);

                 
            }
        }

        private void StartJob(string jobName)
        {

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