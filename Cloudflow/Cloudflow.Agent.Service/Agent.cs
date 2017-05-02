using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Cloudflow.Agent.Service
{
    public class Agent
    {
        #region Private Members
        private int _runCounter = 1;
        #endregion

        #region Properties
        public List<Job> Jobs { get; }

        public TaskScheduler TaskScheduler { get; }
        #endregion

        #region Constructors
        public Agent()
        {
            this.Jobs = new List<Job>();
            
        }
        #endregion

        #region Private Methods
        private void LoadJobs()
        {
            this.Jobs.Clear();
            var job = Job.CreateTestJob("Test Job 1");
            job.JobTriggerFired += Job_JobTriggerFired;
            this.Jobs.Add(job);

            job = Job.CreateTestJob("Test Job 2");
            job.JobTriggerFired += Job_JobTriggerFired;
            this.Jobs.Add(job);
        }

        private void Job_JobTriggerFired(Job job, Trigger trigger, Dictionary<string, object> triggerData)
        {
            Run run = new Run(string.Format("{0} Run {1}", job.Name, _runCounter++), job, triggerData);
            run.Start();
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            LoadJobs();
            foreach (var job in this.Jobs)
            {
                job.Start();
            }
        }

        public void Stop()
        {
            foreach (var job in this.Jobs)
            {
                job.Stop();
            }
        }
        #endregion
    }
}
